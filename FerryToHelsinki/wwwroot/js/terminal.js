﻿window.terminalFunctions = {
    animateTerminalOpened: async function (...messageElements) {
        for (i = 0; i < messageElements.length; i++) {
            await this.animateTextAsync(messageElements[i][0], messageElements[i][1], 1);
        }
        return true; // denotes when the terminal has rendered everything required
    },
    animateMessage: async function (messageContents) {
        await this.animateTextAsync(messageContents, "message", 200);
        return true;
    },
    animateResponse: async function (responseContents, messagePrefix) {
        await this.animateTextAsync('\n' + responseContents, "message", 1);
        await this.animateTextAsync('\n' + messagePrefix, "message", 1);
    },
    animateTextAsync: function (messageContents, elementId, typingSpeed) {
        if (!messageContents) {
            return false;
        }
        return new Promise(resolve => {
            var messageSpan = document.getElementById(elementId);
            let charCount = 0;
            charInterval = setInterval(() => {
                messageSpan.textContent += messageContents[charCount];
                charCount++;

                if (charCount >= messageContents.length) {
                    clearInterval(charInterval);
                    resolve();
                }
            }, typingSpeed)
        }).then(() => {
            return true;
        });
    },
    animateFerries: function (...ferryFrames) {
        var ferryImage = document.getElementById('ferry-image');
        var ferryImageInverted = document.getElementById('ferry-image-inverted');
        let currentFerryFrame = 0;
        setInterval(() => {
            ferryImage.textContent = ferryFrames[currentFerryFrame];
            ferryImageInverted.textContent = ferryFrames[currentFerryFrame];
            currentFerryFrame++;

            if (currentFerryFrame >= ferryFrames.length) {
                currentFerryFrame = 0;
            }

        }, 160)
    }
}

window.ferryMainMenuFunctions = {
    animatePressStart: function () {
        var canvas = document.getElementById('ferry-game-selection');
        var canvasWidth = canvas.clientWidth;
        var canvasHeight = canvas.clientHeight;
        var context = canvas.getContext('2d');

        var pressStart = new Image();
        pressStart.src = "img/pressstartimage.png";

        pressStart.onload = function () {
            var newWidth = pressStart.width / 30;
            var newHeight = pressStart.height / 30;

            var midCanvas = canvasWidth * 0.5;
            var imgMidPoint = newWidth * 0.5;
            var idealCanvasWidth = midCanvas - imgMidPoint;
            var idealCanvasHeight = canvasHeight * 0.2;

            context.drawImage(pressStart, idealCanvasWidth, idealCanvasHeight, newWidth, newHeight);
        }
    }
}