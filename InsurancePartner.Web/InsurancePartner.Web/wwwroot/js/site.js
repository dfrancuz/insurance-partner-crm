// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function(){
    function updateCurrentTime() {
        const currentTimeElement = document.getElementById("current-time");
        currentTimeElement.textContent = new Date().toLocaleTimeString();
    }

    updateCurrentTime();
    setInterval(updateCurrentTime, 1000);
})
