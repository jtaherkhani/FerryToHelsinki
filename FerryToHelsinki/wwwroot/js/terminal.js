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

        // instantiate main menu positions

        const popcornPositions = {
            NEW: "new-game",
            LOAD: "load-game",
            OPTIONS: "options"
        }

        var currentPopcornPosition = popcornPositions.NEW;

        // instantiate options menu positions
        const sodaPositions = {
            STARING: "staring-mode",
            SPEED: "speed-mode",
            COCKS: "translate-cocktails",
            DIFF: "difficulty",
            RETURN: "return"
        }

        var currentSodaPosition = sodaPositions.STARING;

        var optionStatesDictionary = {
            "staring-mode": "Y",
            "speed-mode": "N",
            "translate-cocktails": "N",
            "difficulty": "ad"
        };

        // Instantiate main menu options
        var newGame = createImage('new-game', "img/newgame.png");
        var loadGame = createImage('load-game', "img/loadgame.png");
        var options = createImage('options', "img/options.png");
        var mainMenuSelector = createImage('main-menu-selector', "img/popcornselector.png");

        // Instantiate options menu selections
        var menuOptions = createImage('menu-options', "img/optionsmenu.png");
        var staringMode = createImage('staring-mode', "img/enablestaringmodeoption.png");
        var staringModeYesOption = createImage('yes-value', "img/yesmenuoption.png");
        var staringModeNoOption = createImage('no-value', "img/nomenuoption.png");
        var optionsSelector = createImage('options-selector', "img/popcornselector.png");

        // Shorthand method to create an image
        function createImage(id, src) {
            var newImage = new Image();
            newImage.id = id;
            newImage.src = src;

            return newImage;
        }

        // Instantiate all dimensions as we can't reliably control when each value is loading
        var newGameWidth = 65.055;
        var newGameHeight = 6.944;

        var loadGameWidth = 65.055;
        var loadGameHeight = 6.944;

        var optionsWidth = 56.833;
        var optionsHeight = 7.33;

        var selectorWidth = 10;
        var selectorHeight = 8;

        // Instantiate options menu dimensions
        var menuOptionsWidth = canvasWidth*.91;
        var menuOptionsHeight = canvasHeight * .94;

        var staringModeWidth = 130;
        var staringModeHeight = 8.5;

        var yesOptionWidth = 11;
        var yesOptionHeight = 7;

        var noOptionWidth = 11;
        var noOptionHeight = 7;

        // instantiate where to draw main-menu images
        var newGameMidPoint = newGameWidth * 0.5;
        var canvasImageWidthPosition = midCanvas - newGameMidPoint;
        var optionsMenuWidthPosition = midCanvas - (menuOptionsWidth * .5);
        var mainMenuSelectorWidthPosition = canvasImageWidthPosition - 20;

        var newGameCanvasHeightPosition = idealCanvasHeightStartPoint;
        var loadGameCanvasHeightPosition = newGameCanvasHeightPosition + 20;
        var optionsCanvasHeightPosition = loadGameCanvasHeightPosition + 20;

        // instantiate where to draw options menu images
        var staringModeWidthPosition = menuOptionsWidth * .18 // start from 20% over in the usable space
        var staringModeHeightPosition = menuOptionsHeight * .3 // start from 10% of the screen
        var optionsSelectorWidthPosition = menuOptionsWidth * .11;

        var YesNoOptionWidthPosition = menuOptionsWidth * .9; // start from 80% over in the usable space

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
                var audio = new Audio("sounds/ootselect.wav");
                audio.play();
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

                        document.addEventListener('keydown', logExtendedOptionsActions);

                        break;
                }
            }
        }

        function logExtendedOptionsActions(e) {
            if (e.code == 'Enter') {
                switch (currentSodaPosition) {
                    case (sodaPositions.STARING):
                        optionStatesDictionary["staring-mode"] =
                            optionStatesDictionary["staring-mode"] == "Y"
                                ? "N"
                                : "Y";

                        break;

                    case (sodaPositions.SPEED):
                        optionsStatesDictionary["speed-mode"] =
                            optionsStatesDictionary["speed-mode"] == "Y"
                                ? "N"
                                : "Y";

                        break;

                    case (sodaPositions.COCKS):
                        optionsStatesDictionary["translate-cocktails"] =
                            optionsStatesDictionary["translate-cocktails"] == "Y"
                                ? "N"
                                : "Y";

                        break;

                    case (sodaPositions.DIFF):
                        optionsStatesDictionary["difficulty"] =
                            optionsStatesDictionary["difficulty"] == "ad"
                                ? "ex"
                                : "ad";
                }
            }
            else if (e.code == 'ArrowUp' || e.code == 'ArrowDown') {
                switch (currentSodaPosition) {
                    case sodaPositions.STARING:
                        currentSodaPosition = (e.code == 'ArrowUp' ? sodaPositions.RETURN : sodaPositions.SPEED);
                        break;
                    case sodaPositions.SPEED:
                        currentSodaPosition = (e.code == 'ArrowUp' ? sodaPositions.STARING : sodaPositions.COCKS);
                        break;
                    case sodaPositions.COCKS:
                        currentSodaPosition = (e.code == 'ArrowUp' ? sodaPositions.SPEED : sodaPositions.DIFF);
                        break;
                    case sodaPositions.DIFF:
                        currentSodaPosition = (e.code == 'ArrowUp' ? sodaPositions.COCKS : sodaPositions.RETURN);
                        break;
                    case sodaPositions.RETURN:
                        currentSodaPosition = (e.code == 'ArrowUp' ? sodaPositions.DIFF : sodaPositions.STARING);
                }
            }
        }

        function drawInitialExtendedOptions() {
            menuOptions.onload = function () {
                context.drawImage(menuOptions, optionsMenuWidthPosition, 0, menuOptionsWidth, menuOptionsHeight);
            }

            staringMode.onload = function () {
                context.drawImage(staringMode, staringModeWidthPosition, staringModeHeightPosition, staringModeWidth, staringModeHeight);
            }

            staringModeYesOption.onload = function () {
                context.drawImage(staringModeYesOption, YesNoOptionWidthPosition, staringModeHeightPosition, yesOptionWidth, yesOptionHeight);
            }

            var optionSelectorHeightPosition = findNextOptionSelectorHeightPosition();

            optionsSelector.onload = function () {
                context.drawImage(optionsSelector, optionsSelectorWidthPosition, optionSelectorHeightPosition, selectorWidth, selectorHeight);
            }
        }

        function redrawExtendedOptions() {
            context.drawImage(menuOptions, optionsMenuWidthPosition, 0, menuOptionsWidth, menuOptionsHeight);
            context.drawImage(staringMode, staringModeWidthPosition, staringModeHeightPosition, staringModeWidth, staringModeHeight);

            if (optionStatesDictionary["staring-mode"] == "Y") {
                context.drawImage(staringModeYesOption, YesNoOptionWidthPosition, staringModeHeightPosition, yesOptionWidth, yesOptionHeight);
            }
            else {
                context.drawImage(staringModeNoOption, YesNoOptionWidthPosition, staringModeHeightPosition, noOptionWidth, noOptionHeight);
            }

            var optionSelectorHeightPosition = findNextOptionSelectorHeightPosition();
            context.drawImage(optionsSelector, optionsSelectorWidthPosition, optionSelectorHeightPosition, selectorWidth, selectorHeight);

        }

        function findNextOptionSelectorHeightPosition() {
            var optionSelectorHeightPosition = 0;

            switch (currentSodaPosition) {
                case sodaPositions.STARING:
                    optionSelectorHeightPosition = staringModeHeightPosition;
                    break;
            }

            return optionSelectorHeightPosition;
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

            mainMenuSelector.onload = function () {
                var selectorCanvasHeightPosition = findNextSelectorPosition();
                context.drawImage(selector, mainMenuSelectorWidthPosition, selectorCanvasHeightPosition, selectorWidth, selectorHeight);
            }
        }

        function redrawMainMenu() {
            context.drawImage(newGame, canvasImageWidthPosition, newGameCanvasHeightPosition, newGameWidth, newGameHeight);
            context.drawImage(loadGame, canvasImageWidthPosition, loadGameCanvasHeightPosition, loadGameWidth, loadGameHeight);
            context.drawImage(options, canvasImageWidthPosition, optionsCanvasHeightPosition, optionsWidth, optionsHeight);

            var selectorCanvasHeightPosition = findNextSelectorPosition();
            context.drawImage(mainMenuSelector, mainMenuSelectorWidthPosition, selectorCanvasHeightPosition, selectorWidth, selectorHeight);
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