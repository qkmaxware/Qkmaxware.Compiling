function BlazorReadLocalStorage(name) {
    return window.localStorage.getItem(name);
}
function BlazorWriteLocalStorage(name, content) {
    return window.localStorage.setItem(name, content);
}
function BlazorListLocalStorage() {
    return Object.keys(window.localStorage);
}