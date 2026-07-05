const list = document.getElementById("detectedItemsList");

function createChecklistItem(streamId, label) {
	const li = document.createElement("li");
	const checkbox = document.createElement("input");
	checkbox.type = "checkbox";
	checkbox.id = `check-${streamId}-${label}`;
	checkbox.value = label;
	checkbox.disabled = true;
	const labelEl = document.createElement("label");
	labelEl.htmlFor = `check-${streamId}-${label}`;
	labelEl.textContent = label;
	li.appendChild(checkbox);
	li.appendChild(labelEl);
	return { li, checkbox };
}

function populateChecklist(streamId, labels) {
	const liveChecklistItems = document.getElementById(
		"detectedItemsChecklist",
	);
	liveChecklistItems.innerHTML = "";
	labels.forEach((label) => {
		const { li } = createChecklistItem(streamId, label);
		liveChecklistItems.appendChild(li);
	});
}

function addDetectedItemCheckbox(streamId, label) {
	const liveChecklistItems = document.getElementById(
		"detectedItemsChecklist",
	);
	let checkbox = document.getElementById(`check-${streamId}-${label}`);
	if (!checkbox) {
		const created = createChecklistItem(streamId, label);
		liveChecklistItems.appendChild(created.li);
		checkbox = created.checkbox;
	}
	checkbox.checked = true;
}

const rovStreamConnection = new signalR.HubConnectionBuilder()
	.withUrl("/rovstreamhub")
	.build();

rovStreamConnection.on("StreamStarted", (data) => {
	document.getElementById("currentStreamingStatus").textContent =
		`Live #${data.streamId} — ` + data.title;
	document.getElementById("liveIndicator").classList.remove("hidden");
	document.getElementById("liveSection").classList.remove("hidden");
	document.getElementById("liveStreamFeed").src = data.sourceUrl;

	populateChecklist(data.streamId, data.detectedItemsUniqueLabels);
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
	addDetectedItemCheckbox(data.streamId, data.label);

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
