window.terminalFunctions = {
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
    animateCanvas: function () {
        var canvas = document.getElementById('ferry-game-selection');
        var canvasWidth = canvas.width;
        var midCanvas = canvasWidth * 0.5;
        var canvasHeight = canvas.height;
        var idealCanvasHeightStartPoint = canvasHeight * 0.2;
        var context = canvas.getContext('2d');
        var refreshMenuIntervalId = 0;
        var optionsMenuIntervalId = 0;

        drawPressPlay();

        var isDrawn = true;

        var pressPlayDrawing = setInterval(() => {
            if (isDrawn) {
                context.clearRect(0, 0, canvasWidth, canvasHeight);
            }
            else {
                drawPressPlay();
            }
            isDrawn = !isDrawn;
        }, 600);

        document.addEventListener('keypress', logPressStart);

        function logPressStart(e) {
            if (e.code == 'Space') {
                var audio = new Audio("sounds/ootpressstart.wav");
                audio.play();
                
                document.removeEventListener('keypress', logPressStart);
                clearInterval(pressPlayDrawing);

                clearCanvas();
                drawInitialMainMenu();

                refreshMenuIntervalId = setInterval(() => {
                    clearCanvas();
                    redrawMainMenu();
                }, 33);

                document.addEventListener('keydown', logMainMenuActions);
            }
        }

        // instantiate main menu constructs

        const popcornPositions = {
            NEW: "new-game",
            LOAD: "load-game",
            OPTIONS: "options",
        }

        var currentPopcornPosition = popcornPositions.NEW;

        var newGame = new Image();
        newGame.id = 'new-game';
        newGame.src = "img/newgame.png";

        var loadGame = new Image();
        loadGame.id = 'load-game';
        loadGame.src = "img/loadgame.png";

        var options = new Image();
        options.id = 'options';
        options.src = 'img/options.png';

        var selector = new Image();
        selector.id = 'selector';
        selector.src = 'img/popcornselector.png';

        var menuOptions = new Image();
        menuOptions.id = 'menu-options';
        menuOptions.src = 'img/optionsmenu.png';

        // instantiate all dimensions as we can't reliably control when each value is loading
        var newGameWidth = 65.055;
        var newGameHeight = 6.944;

        var loadGameWidth = 65.055;
        var loadGameHeight = 6.944;

        var optionsWidth = 56.833;
        var optionsHeight = 7.33;

        var selectorWidth = 10;
        var selectorHeight = 8;

        var menuOptionsWidth = canvasWidth*.91;
        var menuOptionsHeight = canvasHeight*.94;

        // instantiate where to draw images
        var newGameMidPoint = newGameWidth * 0.5;
        var canvasImageWidthPosition = midCanvas - newGameMidPoint;
        var optionsMenuWidthPosition = midCanvas - (menuOptionsWidth * .5);
        var selectorWidthPosition = canvasImageWidthPosition - 20;



        var newGameCanvasHeightPosition = idealCanvasHeightStartPoint;
        var loadGameCanvasHeightPosition = newGameCanvasHeightPosition + 20;
        var optionsCanvasHeightPosition = loadGameCanvasHeightPosition + 20;


        function logMainMenuActions(e) {
            if (e.code == 'ArrowUp' || e.code == 'ArrowDown') {
                switch (currentPopcornPosition) {
                    case popcornPositions.NEW:
                        currentPopcornPosition = (e.code == 'ArrowUp' ? popcornPositions.OPTIONS : popcornPositions.LOAD);
                        break;
                    case popcornPositions.LOAD:
                        currentPopcornPosition = (e.code == 'ArrowUp' ? popcornPositions.NEW : popcornPositions.OPTIONS);
                        break;
                    case popcornPositions.OPTIONS:
                        currentPopcornPosition = (e.code == 'ArrowUp' ? popcornPositions.LOAD : popcornPositions.NEW);
                        break;
                }
            }
            else if (e.code == 'Enter') {
                clearInterval(refreshMenuIntervalId);
                document.removeEventListener('keydown', logMainMenuActions);

                clearCanvas();
                switch (currentPopcornPosition) {
                    case (popcornPositions.OPTIONS):
                        drawInitialExtendedOptions();

                        optionsMenuIntervalId = setInterval(() => {
                            clearCanvas();
                            redrawExtendedOptions();
                        }, 33);

                        break;
                }
            }
        }

        function drawInitialExtendedOptions() {
            menuOptions.onload = function () {
                context.drawImage(menuOptions, optionsMenuWidthPosition, 0, menuOptionsWidth, menuOptionsHeight);
            }
        }

        function redrawExtendedOptions() {
            context.drawImage(menuOptions, optionsMenuWidthPosition, 0, menuOptionsWidth, menuOptionsHeight);
        }

        function clearCanvas() {
            context.clearRect(0, 0, canvasWidth, canvasHeight);
        }

        function drawInitialMainMenu() {
            newGame.onload = function () {
                    context.drawImage(newGame, canvasImageWidthPosition, newGameCanvasHeightPosition, newGameWidth, newGameHeight);
            }

            loadGame.onload = function () {
                context.drawImage(loadGame, canvasImageWidthPosition, loadGameCanvasHeightPosition, loadGameWidth, loadGameHeight);
            }

            options.onload = function () {
                context.drawImage(options, canvasImageWidthPosition, optionsCanvasHeightPosition, optionsWidth, optionsHeight);
            }

            selector.onload = function () {
                var selectorCanvasHeightPosition = findNextSelectorPosition();
                context.drawImage(selector, selectorWidthPosition, selectorCanvasHeightPosition, selectorWidth, selectorHeight);
            }
        }

        function redrawMainMenu() {
            context.drawImage(newGame, canvasImageWidthPosition, newGameCanvasHeightPosition, newGameWidth, newGameHeight);
            context.drawImage(loadGame, canvasImageWidthPosition, loadGameCanvasHeightPosition, loadGameWidth, loadGameHeight);
            context.drawImage(options, canvasImageWidthPosition, optionsCanvasHeightPosition, optionsWidth, optionsHeight);

            var selectorCanvasHeightPosition = findNextSelectorPosition();
            context.drawImage(selector, selectorWidthPosition, selectorCanvasHeightPosition, selectorWidth, selectorHeight);
        }

        function findNextSelectorPosition() {
            var selectorHeightPosition = 0;

            switch (currentPopcornPosition) {
                case popcornPositions.NEW:
                    selectorHeightPosition = newGameCanvasHeightPosition;
                    break;
                case popcornPositions.LOAD:
                    selectorHeightPosition = loadGameCanvasHeightPosition;
                    break;
                case popcornPositions.OPTIONS:
                    selectorHeightPosition = optionsCanvasHeightPosition;
                    break;
            }

            return selectorHeightPosition;
        }

        function drawPressPlay() {
            var pressStart = new Image();
            pressStart.id = 'press-start';
            pressStart.src = "img/pressstartimage.png";

            pressStart.onload = function () {
                var newWidth = pressStart.width / 22;
                var newHeight = pressStart.height / 22;

                var imgMidPoint = newWidth * 0.5;
                var idealCanvasWidth = midCanvas - imgMidPoint;

                context.drawImage(pressStart, idealCanvasWidth, idealCanvasHeightStartPoint, newWidth, newHeight);
            }
        }

        

        
    }
}