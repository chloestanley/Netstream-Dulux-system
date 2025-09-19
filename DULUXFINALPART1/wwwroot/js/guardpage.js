let canvas = document.getElementById('signatureCanvas');
let ctx = canvas.getContext('2d');
let isDrawing = false;

canvas.addEventListener('mousedown', (e) => {
    isDrawing = true;
    ctx.beginPath();
    ctx.moveTo(e.offsetX, e.offsetY);
});

canvas.addEventListener('mousemove', (e) => {
    if (isDrawing) {
        ctx.lineTo(e.offsetX, e.offsetY);
        ctx.stroke();
    }
});

canvas.addEventListener('mouseup', () => {
    isDrawing = false;
});

function saveSignature() {
    let signatureData = canvas.toDataURL(); // Convert canvas to Base64
    document.getElementById('SignatureData').value = signatureData;
}

function clearCanvas() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    document.getElementById('SignatureData').value = ""; // Clear stored data
}
