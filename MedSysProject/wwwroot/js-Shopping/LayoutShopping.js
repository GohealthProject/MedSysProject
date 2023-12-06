function search() {
    const key = document.querySelector("#SearchKey").value;
    const url = "/Shopping/KeySearch?" + key;
    window.location.href = url;
}