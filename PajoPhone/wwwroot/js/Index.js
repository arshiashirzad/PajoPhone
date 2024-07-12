var open=true;
function OpenFilterBar(){
    if(open===true) {
        document.getElementById('filtering-bar').classList.remove('hidden');
        document.getElementById('filtering-backdrop').classList.remove('hidden');
        open=false;
    }else {
        document.getElementById('filtering-bar').classList.add('hidden');
        document.getElementById('filtering-backdrop').classList.add('hidden');
        open=true;
    }
}
var incremented = false;
$(window).scroll(function() {
    var scrollPosition = $(window).scrollTop();
    var windowHeight = $(window).height();
    var documentHeight = $(document).height();

    if (!incremented && scrollPosition + windowHeight >= documentHeight * 0.9) {
        var pageNo = parseInt($('#pagination').val(), 10);
        $('#pagination').val(pageNo + 1).change();
        incremented = true;
        console.log('Page number increased to:', pageNo + 1);
    }

    if (scrollPosition + windowHeight < documentHeight * 0.8) {
        incremented = false;
    }
});
function OpenModal() {
    document.getElementById('myModal').classList.remove('hidden');
    document.getElementById('backDrop').classList.remove('hidden');
}
function CloseModal() {
    document.getElementById("modal-container").innerHTML = "";
}
function UpdatePrice(value) {
    document.getElementById("minPrice").textContent = "$" + value;
}
window.onclick = function (event) {
    if (event.target == document.getElementById('myModal')) {
        CloseModal();
    }
}
