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
    animateCanvas: function (netReference) {
        var ferryStartedReference = netReference;
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
            if (e.code == 'Enter') {
                pressStartAudio.play();
                
                document.removeEventListener('keypress', logPressStart);
                clearInterval(pressPlayDrawing);

                OpenMainMenu();
            }
        }

        function OpenMainMenu() {
            clearCanvas();
            drawInitialMainMenu();

            refreshMenuIntervalId = setInterval(() => {
                clearCanvas();
                redrawMainMenu();
            }, 33);

            document.addEventListener('keydown', logMainMenuActions);
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

        // Instantiate Sounds
        var menuSelectAudio = new Audio("sounds/ootselect.wav");
        var pressStartAudio = new Audio("sounds/ootpressstart.wav");

        // Instantiate main menu options
        var newGame = createImage('new-game', "img/newgame.png");
        var loadGame = createImage('load-game', "img/loadgame.png");
        var options = createImage('options', "img/options.png");
        var mainMenuSelector = createImage('main-menu-selector', "img/popcornselector.png");

        // Instantiate options menu selections
        var menuOptions = createImage('menu-options', "img/optionsmenu.png");

        var staringMode = createImage('staring-mode', "img/enablestaringmodeoption.png");
        var staringModeYesOption = createImage('yes-value-stare', "img/yesmenuoption.png");
        var staringModeNoOption = createImage('no-value-stare', "img/nomenuoption.png");

        var speedRunMode = createImage('speed-run', "img/speedrunmodeoption.png");
        var speedRunModeYesOption = createImage('yes-value-speed', "img/yesmenuoption.png");
        var speedRunModeNoOption = createImage('no-value-speed', "img/nomenuoption.png");

        var translateCocks = createImage('translate-cocks', "img/translatecocktailsoption.png");
        var translateCocksYesOption = createImage('yes-value-translate', "img/yesmenuoption.png");
        var translateCocksNoOption = createImage('no-value-translate', "img/nomenuoption.png");

        var difficulty = createImage('difficulty', "img/difficultyoption.png");
        var adequateDifficulty = createImage('adequate-difficulty', "img/adequatedifficulty.png");
        var excessiveDifficulty = createImage('excessive-difficulty', "img/excessivedifficulty.png");

        var returnToMainMenu = createImage('return-to-main-menu', "img/returntomainmenu.png");

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

        var staringModeWidth = 143;
        var staringModeHeight = 8.5;

        var speedRunModeWidth = 115;
        var speedRunModeHeight = 8.5;

        var translateCocksWidth = 200;
        var translateCocksHeight = 8.5;

        var difficultyWidth = 65;
        var difficultyHeight = 8.5;

        var adequateDifficultyWidth = 65;
        var adequateDifficultyHeight = 8.5;

        var excessiveDifficultyWidth = 65;
        var excessiveDifficultyHeight = 8.5;

        var returnToMainMenuWidth = 170;
        var returnToMainMenuHeight = 8.5;

        var yesOptionWidth = 11;
        var yesOptionHeight = 8.5;

        var noOptionWidth = 11;
        var noOptionHeight = 8.5;

        // instantiate where to draw main-menu images
        var newGameMidPoint = newGameWidth * .5;
        var canvasImageWidthPosition = midCanvas - newGameMidPoint;
        var optionsMenuWidthPosition = midCanvas - (menuOptionsWidth * .5);
        var mainMenuSelectorWidthPosition = canvasImageWidthPosition - 20;

        var newGameCanvasHeightPosition = idealCanvasHeightStartPoint;
        var loadGameCanvasHeightPosition = newGameCanvasHeightPosition + 20;
        var optionsCanvasHeightPosition = loadGameCanvasHeightPosition + 20;

        // instantiate where to draw options menu images
        var staringModeWidthPosition = menuOptionsWidth * .12
        var optionsSelectorWidthPosition = menuOptionsWidth * .07;
        var adequateDifficultyWidthPosition = (menuOptionsWidth* .95) - adequateDifficultyWidth;
        var excessiveDifficultyWidthPosition = (menuOptionsWidth * .95) - excessiveDifficultyWidth;

        var returnToMainMenuMidPoint = returnToMainMenuWidth * .5;
        var returnToMainMenuWidthPosition = midCanvas - returnToMainMenuMidPoint

        var staringModeHeightPosition = menuOptionsHeight * .3 
        var speedRunModeHeightPosition = findNextOptionHeightPosition(staringModeHeightPosition);
        var translateCocksHeightPosition = findNextOptionHeightPosition(speedRunModeHeightPosition);
        var difficultyHeightPosition = findNextOptionHeightPosition(translateCocksHeightPosition);
        var returnToMainMenuHeightPosition = menuOptionsHeight * .8; // put to the bottom of the screen

        function findNextOptionHeightPosition(lastHeightPosition) {
            return lastHeightPosition + 16;
        }

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
                menuSelectAudio.play();
                clearInterval(refreshMenuIntervalId);
                document.removeEventListener('keydown', logMainMenuActions);

                clearCanvas();
                switch (currentPopcornPosition) {
                    case (popcornPositions.NEW):
                        ferryStartedReference.invokeMethodAsync("NewGameStartAsync", JSON.stringify(optionStatesDictionary));
                        break;

                    case (popcornPositions.OPTIONS):
                        currentSodaPosition = sodaPositions.STARING;
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
                        optionStatesDictionary["speed-mode"] =
                            optionStatesDictionary["speed-mode"] == "Y"
                                ? "N"
                                : "Y";

                        break;

                    case (sodaPositions.COCKS):
                        optionStatesDictionary["translate-cocktails"] =
                            optionStatesDictionary["translate-cocktails"] == "Y"
                                ? "N"
                                : "Y";

                        break;

                    case (sodaPositions.DIFF):
                        optionStatesDictionary["difficulty"] =
                            optionStatesDictionary["difficulty"] == "ad"
                                ? "ex"
                                : "ad";

                        break;

                    case (sodaPositions.RETURN):
                        menuSelectAudio.play();
                        currentPopcornPosition = popcornPositions.NEW;
                        clearInterval(optionsMenuIntervalId);
                        document.removeEventListener('keydown', logExtendedOptionsActions);
                        OpenMainMenu();

                        break;
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

            speedRunMode.onload = function () {
                context.drawImage(speedRunMode, staringModeWidthPosition, speedRunModeHeightPosition, speedRunModeWidth, speedRunModeHeight);
            }

            speedRunModeNoOption.onload = function () {
                context.drawImage(speedRunModeNoOption, YesNoOptionWidthPosition, speedRunModeHeightPosition, noOptionWidth, noOptionHeight);
            }

            translateCocks.onload = function () {
                context.drawImage(translateCocks, staringModeWidthPosition, translateCocksHeightPosition, translateCocksWidth, translateCocksHeight);
            }

            translateCocksNoOption.onload = function () {
                context.drawImage(translateCocksNoOption, YesNoOptionWidthPosition, translateCocksHeightPosition, noOptionWidth, noOptionHeight);
            }

            difficulty.onload = function () {
                context.drawImage(difficulty, staringModeWidthPosition, difficultyHeightPosition, difficultyWidth, difficultyHeight);
            }

            adequateDifficulty.onload = function () {
                context.drawImage(adequateDifficulty, adequateDifficultyWidthPosition, difficultyHeightPosition, adequateDifficultyWidth, adequateDifficultyHeight);
            }

            returnToMainMenu.onload = function () {
                context.drawImage(returnToMainMenu, returnToMainMenuWidthPosition, returnToMainMenuHeightPosition, returnToMainMenuWidth, returnToMainMenuHeight);
            }

            var optionSelectorHeightPosition = findNextOptionSelectorHeightPosition();

            optionsSelector.onload = function () {
                context.drawImage(optionsSelector, optionsSelectorWidthPosition, optionSelectorHeightPosition, selectorWidth, selectorHeight);
            }
        }

        function redrawExtendedOptions() {
            context.drawImage(menuOptions, optionsMenuWidthPosition, 0, menuOptionsWidth, menuOptionsHeight);

            context.drawImage(staringMode, staringModeWidthPosition, staringModeHeightPosition, staringModeWidth, staringModeHeight);
            drawYesNoOption("staring-mode", staringModeYesOption, staringModeNoOption, staringModeHeightPosition);

            context.drawImage(speedRunMode, staringModeWidthPosition, speedRunModeHeightPosition, speedRunModeWidth, speedRunModeHeight);
            drawYesNoOption("speed-mode", speedRunModeYesOption, speedRunModeNoOption, speedRunModeHeightPosition);

            context.drawImage(translateCocks, staringModeWidthPosition, translateCocksHeightPosition, translateCocksWidth, translateCocksHeight);
            drawYesNoOption("translate-cocktails", translateCocksYesOption, translateCocksNoOption, translateCocksHeightPosition);

            context.drawImage(difficulty, staringModeWidthPosition, difficultyHeightPosition, difficultyWidth, difficultyHeight);
            if (optionStatesDictionary["difficulty"] == "ad") {
                context.drawImage(adequateDifficulty, adequateDifficultyWidthPosition, difficultyHeightPosition, adequateDifficultyWidth, adequateDifficultyHeight);
            }
            else {
                context.drawImage(excessiveDifficulty, excessiveDifficultyWidthPosition, difficultyHeightPosition, excessiveDifficultyWidth, excessiveDifficultyHeight);
            }

            context.drawImage(returnToMainMenu, returnToMainMenuWidthPosition, returnToMainMenuHeightPosition, returnToMainMenuWidth, returnToMainMenuHeight);

            function drawYesNoOption(key, yesOptionToDraw, noOptionToDraw, heightPosition) {
                if (optionStatesDictionary[key] == "Y") {
                    context.drawImage(yesOptionToDraw, YesNoOptionWidthPosition, heightPosition, yesOptionWidth, yesOptionHeight);
                }
                else {
                    context.drawImage(noOptionToDraw, YesNoOptionWidthPosition, heightPosition, noOptionWidth, noOptionHeight);
                }
            }

            var optionSelectorHeightPosition = findNextOptionSelectorHeightPosition();
            var selectorWidthPosition = currentSodaPosition == sodaPositions.RETURN ? returnToMainMenuWidthPosition - 20 : optionsSelectorWidthPosition;

            context.drawImage(optionsSelector, selectorWidthPosition, optionSelectorHeightPosition, selectorWidth, selectorHeight);
        }

        function findNextOptionSelectorHeightPosition() {
            var optionSelectorHeightPosition = 0;

            switch (currentSodaPosition) {
                case sodaPositions.STARING:
                    optionSelectorHeightPosition = staringModeHeightPosition;
                    break;

                case sodaPositions.SPEED:
                    optionSelectorHeightPosition = speedRunModeHeightPosition;
                    break;

                case sodaPositions.COCKS:
                    optionSelectorHeightPosition = translateCocksHeightPosition;
                    break;

                case sodaPositions.DIFF:
                    optionSelectorHeightPosition = difficultyHeightPosition;
                    break;

                case sodaPositions.RETURN:
                    optionSelectorHeightPosition = returnToMainMenuHeightPosition;
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

window.ferryLoadingFunctions = {
    animateLoading: function () {
        var canvas = document.getElementById('ferry-loading');
        var canvasWidth = canvas.width;
        var canvasHeight = canvas.height;
        var midCanvasWidth = canvasWidth * 0.5;
        var midCanvasHeight = canvasHeight * 0.5;

        var context = canvas.getContext('2d');

        // Instantiate image transitional states
        var l_state = createImage('l', "img/loading/L.png");
        var lo_state = createImage('lo', "img/loading/LO.png");
        var loa_state = createImage('loa', "img/loading/LOA.png");
        var load_state = createImage('load', "img/loading/LOAD.png");
        var loadi_state = createImage('loadi', "img/loading/LOADI.png");
        var loadin_state = createImage('loadin', "img/loading/LOADIN.png");
        var loading_state = createImage('loading', "img/loading/LOADING.png");

        function createImage(id, src) {
            var newImage = new Image();
            newImage.id = id;
            newImage.src = src;

            return newImage;
        }

        // Should all be the same width and height
        var imageWidth = 180;
        var imageHeight = 60;

        // Instantiate where on the screen we should draw
        var loadingMidPoint = imageWidth * .5;
        var loadingHeightMidPoint = imageHeight * .5;
        var canvasImageWidthPosition = midCanvasWidth - loadingMidPoint;
        var canvasImageHeightPosition = midCanvasHeight - loadingHeightMidPoint;

        // Instantiate a constant representing the states of the loading control
        const loadingStates = {
            L: "l",
            LO:"lo",
            LOA:"loa",
            LOAD: "load",
            LOADI: "loadi",
            LOADIN: "loadin",
            LOADING: "loading"
        }

        var currentLoadingState = loadingStates.L;

        drawInitialLoading();

        var cycleLoadingSteps = setInterval(() => {
            clearCanvas();
            redrawInitialLoading();
        }, 200);


        function drawInitialLoading() {
            l_state.onload = function () {
                context.drawImage(l_state, canvasImageWidthPosition, canvasImageHeightPosition, imageWidth, imageHeight);
            }
        }

        function redrawInitialLoading() {
            var nextLoadingStateImage = setNextLoadingStateToDraw();
            context.drawImage(nextLoadingStateImage, canvasImageWidthPosition, canvasImageHeightPosition, imageWidth, imageHeight);
        }

        function setNextLoadingStateToDraw() {
            switch (currentLoadingState) {
                case loadingStates.L:
                    currentLoadingState = loadingStates.LO;
                    return lo_state;
                    
                case loadingStates.LO:
                    currentLoadingState = loadingStates.LOA;
                    return loa_state;

                case loadingStates.LOA:
                    currentLoadingState = loadingStates.LOAD;
                    return load_state;

                case loadingStates.LOAD:
                    currentLoadingState = loadingStates.LOADI;
                    return loadi_state;

                case loadingStates.LOADI:
                    currentLoadingState = loadingStates.LOADIN;
                    return loadin_state;

                case loadingStates.LOADIN:
                    currentLoadingState = loadingStates.LOADING;
                    return loading_state;

                case loadingStates.LOADING:
                    currentLoadingState = loadingStates.L;
                    return l_state;
            }
        }

        function clearCanvas() {
            context.clearRect(0, 0, canvasWidth, canvasHeight);
        }
    }
}

window.ferryGameplayFunctions = {
    animateTimeline: function (ferryTimelineAscii) {
        var ferryTimelineImage = document.getElementById('ferry-timeline');

        var currentFerryTimelineAscii = ferryTimelineAscii;
        var totalExecutions = 89 // based on characters on the screen
        var executionToStartCountdown = 74
        var currentExecutions = 0
        var lowerDeckLength = 21
        var midDeckLength = 21
        var upperDeckLength = 18
        var smokeStackLength = 15

        var timelineInterval = setInterval(() => {
            var boatPositionEnd = currentFerryTimelineAscii.length - 110 // current length for the water in the ascii drawing
            var lowerDeckPositionStart = boatPositionEnd - lowerDeckLength 
            var midDeckPositionStart = lowerDeckPositionStart - midDeckLength
            var upperDeckPositionStart = midDeckPositionStart - upperDeckLength
            var smokeStackPositionStart = upperDeckPositionStart - smokeStackLength
            var asciiPriorToSmokeStackAdd = currentFerryTimelineAscii.slice(0, smokeStackPositionStart)
            var asciiSmokeStack = currentFerryTimelineAscii.slice(smokeStackPositionStart, upperDeckPositionStart)
            var asciiUpperDeck = currentFerryTimelineAscii.slice(upperDeckPositionStart, midDeckPositionStart)
            var asciiMidDeck = currentFerryTimelineAscii.slice(midDeckPositionStart, lowerDeckPositionStart)
            var asciiLowerDeck = currentFerryTimelineAscii.slice(lowerDeckPositionStart, boatPositionEnd)
            var asciiWater = currentFerryTimelineAscii.slice(boatPositionEnd, currentFerryTimelineAscii.length) // take out the two characters from the end

            asciiCharactersToAdd = " " 

            currentFerryTimelineAscii = asciiPriorToSmokeStackAdd + asciiCharactersToAdd + asciiSmokeStack + asciiCharactersToAdd + asciiUpperDeck + asciiCharactersToAdd + asciiMidDeck + asciiCharactersToAdd + asciiLowerDeck + asciiWater
            ferryTimelineImage.textContent = currentFerryTimelineAscii;

            currentExecutions++
            lowerDeckLength++
            midDeckLength++
            upperDeckLength++
            smokeStackLength++

            if (currentExecutions == executionToStartCountdown) {
                setTimeout(countDown(), 1483) // based on the outer interval length
            }

            if (currentExecutions >= totalExecutions) {
                clearInterval(timelineInterval)
            }

        }, 89*1000)

        function countDown() {
            var countdownMinutesLength = 10
            var currentTime = Date.parse(new Date());
            var deadline = new Date(currentTime + countdownMinutesLength * 60 * 1000);
            var countdown = document.getElementById("dramatic-countdown-div");

            var countdownId = setInterval(() => {
                countdown.style.display = "inline-block";
                var timeFromStart = Date.parse(deadline) - Date.parse(new Date()); // new date gets current date + time
                var seconds = Math.floor((timeFromStart / 1000) % 60); // retrieves the current second that the time has dropped
                var minutes = Math.floor((timeFromStart / 1000 / 60) % 60); // retrieves the current minute that the time has dropped

                countdown.innerHTML = minutes + ':';

                if (seconds < 10) {
                    countdown.innerHTML += 0;
                }

                countdown.innerHTML += seconds;

                if (timeFromStart <= 0) {
                    clearInterval(countdownId)
                }

            }, 1000)
        }
    }
}