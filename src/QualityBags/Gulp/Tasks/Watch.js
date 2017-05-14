var gulp = require("gulp");
var watch = require("gulp-watch");
var browserSync = require("browser-sync").create();

gulp.task("watch", function () {
    browserSync.init({
        notify: true,
        proxy: "http://localhost:57009"
    });

    watch("./wwwroot/css/**/*.less", function () {
        gulp.start("cssInject");
    });
});

gulp.task('cssInject', ['less'], function () {
    return gulp.src('./wwwroot/css/styles.css')
		.pipe(browserSync.stream());
});

gulp.task('scriptsRefresh', ['scripts'], function () {
    browserSync.reload();
});