export function drawTreemap(canvasId,nodes) {
    const canvas = document.getElementById(canvasId);
    canvas.width = canvas.offsetWidth;
    canvas.height = canvas.offsetHeight;

    const ctx = canvas.getContext("2d");

    ctx.fillStyle = "black";
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    ctx.strokeStyle = "gray";
    ctx.lineWidth = 1;
    drawNode(ctx, nodes);
}

function drawNode(ctx, nodes) {
    nodes.forEach(node => {
        ctx.fillStyle = node.color;
        ctx.fillRect(node.x, node.y, node.width, node.height);
        ctx.strokeRect(node.x, node.y, node.width, node.height);
        ctx.fillStyle = "white";
        ctx.fillText(node.label, node.x, node.y+10);
        drawNode(ctx, node.items);
    });
}

export function initializeResizeListener(canvasId, nodes) {
    window.addEventListener('resize', () => {
        drawTreemap(canvasId, nodes);
    });
}

export function getCanvasSize(canvasId) {
    const canvas = document.getElementById(canvasId);
    return {
        width: canvas.offsetWidth,
        height: canvas.offsetHeight
    };
}