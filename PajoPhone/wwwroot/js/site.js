
function openModal() {
    document.getElementById('myModal').classList.remove('hidden');
    document.getElementById('backDrop').classList.remove('hidden');
}
function closeModal() {
    document.getElementById( 'myModal').classList.add('hidden');
    document.getElementById( 'backDrop').classList.add('hidden');
}
window.onclick = function(event) {
    if (event.target == document.getElementById('myModal')) {
        closeModal();
    }
}
