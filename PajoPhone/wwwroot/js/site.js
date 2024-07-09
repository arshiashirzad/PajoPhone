function openModal() {
    document.getElementById('myModal').classList.remove('hidden');
    document.getElementById('backDrop').classList.remove('hidden');
}
function closeModal() {
    document.getElementById("modal-container").innerHTML = "";
}
window.onclick = function(event) {
    if (event.target == document.getElementById('myModal')) {
        closeModal();
    }
}
