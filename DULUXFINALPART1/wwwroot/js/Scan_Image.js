document.getElementById("updateButton").addEventListener("click", function () {
    let selectedIds = [];
    document.querySelectorAll("input[type=checkbox]:checked").forEach((checkbox) => {
        selectedIds.push(parseInt(checkbox.value));
    });

    fetch("/Scan_Image/UpdateCheckboxes", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(selectedIds),
    })
        .then(response => response.json())
        .then(data => {
            alert(data.message);
            location.reload(); // Refresh to reflect changes
        })
        .catch(error => console.error("Error:", error));
});

