mergeInto(LibraryManager.library, {

    SaveHighScore: function (score) {
        window.localStorage.setItem('birdAndBeansHighScore', score);
    },

    LoadHighScore: function () {
        var score = window.localStorage.getItem('birdAndBeansHighScore');

        if(score == null) {
            return 0;
        }

        return parseInt(score);
    }

});
