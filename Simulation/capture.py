from datetime import datetime, UTC
import os
from flask import Flask, Response
import threading
import cv2
import requests
import imageio

app = Flask(__name__)
current_frame = None

lock = threading.Lock()
command = ""
stream = False

detected_items = []

def generate_frames():
    while True:
        if current_frame is None:
            continue
        with lock:
            _, buffer = cv2.imencode(".jpg", current_frame)
            frame_bytes = buffer.tobytes()
        yield(
            b"--frame\r\n"
            b"Content-Type: image/jpeg\r\n\r\n" 
            + frame_bytes
            + b"\r\n"
        ) 

@app.route("/stream")
def stream():
    return Response(generate_frames(), mimetype="multipart/x-mixed-replace; boundary=frame")

def input_commands():
    while True:
        global command
        command = input("> ").strip().lower()

web_app_thread = threading.Thread(target=lambda: app.run(port=5000, use_reloader=False,), daemon=True)
web_app_thread.start()

command_thread = threading.Thread(target=input_commands, daemon=True)
command_thread.start()

while "end" not in command:

    while "start" not in command:
        if "end" in command:
            break
        pass

    if "end" in command:
        break
    
    response = requests.post("http://localhost:5190/api/rovstream/start", json={
        "title": "ROV Live Feed",
        "startTimeStamp": datetime.utcnow().isoformat(),
        "sourceUrl": "http://127.0.0.1:5000/stream"
    })
    stream_id = response.json()["streamId"]

    print(f"Stream #{stream_id} Started")

    filename = f"stream_{stream_id}_{datetime.now().strftime('%Y%m%d_%H%M%S')}.mp4"
    file_path = f"recordings/{filename}"

    fourcc = cv2.VideoWriter_fourcc(*'mp4v')

    capture = cv2.VideoCapture(0)

    fps = capture.get(cv2.CAP_PROP_FPS)
    width = int(capture.get(cv2.CAP_PROP_FRAME_WIDTH))
    height = int(capture.get(cv2.CAP_PROP_FRAME_HEIGHT))

    os.makedirs("recordings", exist_ok=True)
    recording_output = imageio.get_writer(file_path, fps=fps, codec='libx264', quality=8)

    while "stop" not in command:
        ret, frame = capture.read()
        current_frame = cv2.flip(frame, 1)
        recording_output.append_data(cv2.cvtColor(current_frame, cv2.COLOR_BGR2RGB))

        if "x" in command:
            label = command[2:].upper().replace(" ", "_")

            if label not in detected_items:
                snapshot_filename = f"stream_{stream_id}_{label}_detection_{datetime.now().strftime('%Y%m%d_%H%M%S')}.jpg"
                snapshot_file_path = f"snapshots/{snapshot_filename}"

                confidence = 92.01

                os.makedirs("snapshots", exist_ok=True)
                cv2.imwrite(snapshot_file_path, current_frame)

                response = requests.post("http://localhost:5190/api/rovdetecteditem/createSnapshot", json={
                    "streamId": stream_id,
                    "label": label,
                    "confidence": confidence,
                    "detectedAtTimeStamp": datetime.utcnow().isoformat(),
                    "snapshotFileUrl": f"/api/rovdetecteditem/snapshot/{snapshot_filename}",
                })
                detected_item_id = response.json()["detectedItemId"]

                detected_items.append(label)

                print(f"{label} SPOTTED")
            command = ""

    capture.release()
    recording_output.close()

    detected_items = []

    requests.put("http://localhost:5190/api/rovstream/end", json={
        "streamId": stream_id,
        "recordedFileUrl": f"/api/rovstream/recording/{filename}",
        "endTimeStamp": datetime.now(UTC).isoformat()
    })

    print(f"Stream #{stream_id} Ended")
    