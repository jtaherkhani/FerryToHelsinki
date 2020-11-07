window.terminalFunctions = {
    animateTerminalOpened: async function (...messageElements) {
        for (i = 0; i < messageElements.length; i++) {
            await this.animateTextAsync(messageElements[i][0], messageElements[i][1], 1);
        }
        return true; // denotes when the terminal has rendered everything required
    },
    animateMessage: async function (messageContents) {
        setTimeout(async () => {
            await this.animateTextAsync(messageContents, "message", 200);
        }, 1500);
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