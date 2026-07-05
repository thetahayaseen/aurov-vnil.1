const list = document.getElementById("detectedItemsList");

const rovStreamConnection = new signalR.HubConnectionBuilder()
	.withUrl("/rovstreamhub")
	.build();

rovStreamConnection.on("StreamStarted", (data) => {
	document.getElementById("currentStreamingStatus").textContent =
		"Live — " + data.title;
	document.getElementById("liveIndicator").classList.remove("hidden");
	document.getElementById("liveSection").classList.remove("hidden");
	document.getElementById("liveStreamFeed").src = data.sourceUrl;
});

rovStreamConnection.on("StreamEnded", () => {
	document.getElementById("currentStreamingStatus").textContent =
		"Currently not streaming";
	document.getElementById("liveIndicator").classList.add("hidden");
	document.getElementById("liveSection").classList.add("hidden");
	document.getElementById("liveStreamFeed").src = "";
	list.innerHTML = "";
	location.reload();
});

rovStreamConnection.start().catch((err) => console.error(err));

const rovDetectedItemConnection = new signalR.HubConnectionBuilder()
	.withUrl("/rovdetecteditemhub")
	.build();

rovDetectedItemConnection.on("NewItemDetected", (data) => {
	const detectedItem = document.createElement("li");
	detectedItem.className =
		"flex gap-3 items-start bg-zinc-800 rounded-lg p-3";

	const imgTag = document.createElement("img");
	imgTag.src = data.snapshotFileUrl;
	imgTag.className =
		"w-20 h-20 object-cover rounded-md border border-zinc-600";

	const info = document.createElement("div");
	info.innerHTML = `
        <p class="text-white font-medium text-sm">${data.label}</p>
        <p class="text-zinc-500 text-xs">${new Date(data.detectedAtTimeStamp).toLocaleTimeString()}</p>
        <p class="text-zinc-500 text-xs">Confidence: ${data.confidence}%</p>
    `;

	detectedItem.appendChild(imgTag);
	detectedItem.appendChild(info);
	list.appendChild(detectedItem);
});

rovDetectedItemConnection.start().catch((err) => console.error(err));
