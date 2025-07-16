function submitForm(formName) {
    let submitBtn = document.querySelector(`form[name="${formName}"] input[type=submit]`);
    if (submitBtn) {
        submitBtn.click();
    }
}
function isMobile() {
    return window.innerWidth < 576;
}
function focusNearestInput(elementId) {
    var element = document.getElementById(elementId);
    if (element) {
        // This assumes a specific structure for PayloadFieldSelectInput, improve later if needed
        var nearestInput = element.parentElement.parentElement.querySelector('input, select, textarea');
        if (nearestInput) {
            nearestInput.focus();
        }
    }
}

window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer], { type: getMimeType(fileName) });
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}