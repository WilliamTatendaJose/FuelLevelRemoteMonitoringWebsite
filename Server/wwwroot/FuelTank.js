function animateWaterLevel(percentage) {
    const cnt = document.getElementById("count");
    const water = document.getElementById("water");
    let currentPercentage = parseFloat(cnt.innerText);

    const interval = setInterval(function () {
        currentPercentage += 1;
        cnt.innerHTML = currentPercentage;
        water.style.transform = `translate(0, ${100 - currentPercentage}%)`;

        if (currentPercentage >= percentage) {
            clearInterval(interval);
        }
    }, 60);
}
