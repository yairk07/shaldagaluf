document.addEventListener("DOMContentLoaded", function () {
    // גלריית תמונות
    let images = ["pics/gun1.jpg", "pics/gun2.jpg", "pics/knife3.jpg", "pics/knife 4.jpg"];
    let currentIndex = 0;
    let imgElement = document.getElementById("galleryImage");

    function showImage(index) {
        if (imgElement) {
            imgElement.src = images[index];
        }
    }

    document.getElementById("prevBtn").addEventListener("click", function (event) {
        event.preventDefault();
        currentIndex = (currentIndex - 1 + images.length) % images.length;
        showImage(currentIndex);
    });

    document.getElementById("nextBtn").addEventListener("click", function (event) {
        event.preventDefault();
        currentIndex = (currentIndex + 1) % images.length;
        showImage(currentIndex);
    });

    showImage(currentIndex);
});

// ווידג'ט מזג אוויר
!function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (!d.getElementById(id)) {
        js = d.createElement(s);
        js.id = id;
        js.src = 'https://weatherwidget.io/js/widget.min.js';
        fjs.parentNode.insertBefore(js, fjs);
    }
}(document, 'script', 'weatherwidget-io-js');

// חזרה לראש הדף
window.onscroll = function () {
    var scrollToTopBtn = document.getElementById("scrollToTopBtn");
    if (scrollToTopBtn) {
        if (document.body.scrollTop > 200 || document.documentElement.scrollTop > 200) {
            scrollToTopBtn.style.display = "block";
        } else {
            scrollToTopBtn.style.display = "none";
        }
    }
};

function scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

function navigateToURL(url) {
    window.location.href = url;
}

// 🎯 רולטה רוסית
function playRoulette() {
    const word1 = document.getElementById("word1").value.trim();
    const word2 = document.getElementById("word2").value.trim();
    const resultDiv = document.getElementById("rouletteResult");
    const lossSection = document.getElementById("lossSection");
    const loserNameDiv = document.getElementById("loserName");

    if (!word1 || !word2) {
        resultDiv.textContent = "אנא הזן שתי מילים.";
        lossSection.style.display = "none";
        return;
    }

    // תוף עם 3 תאים, כדור באחד מהם
    const bulletChamber = Math.floor(Math.random() * 3) + 1;
    const shot1 = Math.floor(Math.random() * 3) + 1;
    const shot2 = Math.floor(Math.random() * 3) + 1;

    const word1Dead = shot1 === bulletChamber;
    const word2Dead = shot2 === bulletChamber;

    let result = "";

    if (word1Dead && word2Dead) {
        result = "שתי המילים נפגעו! תיקו עקוב מדם.";
        lossSection.style.display = "block";
        loserNameDiv.textContent = "שניהם הפסידו";
    } else if (word1Dead) {
        result = `💀 ${word1} נפגעה. המנצחת היא: ${word2}`;
        lossSection.style.display = "block";
        loserNameDiv.textContent = `${word1} הפסיד`;
    } else if (word2Dead) {
        result = `💀 ${word2} נפגעה. המנצחת היא: ${word1}`;
        lossSection.style.display = "block";
        loserNameDiv.textContent = `${word2} הפסיד`;
    } else {
        result = "שתי המילים שרדו את הירי! נס.";
        lossSection.style.display = "none";
    }

    resultDiv.textContent = result;
}
