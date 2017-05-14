var gulp = require("gulp");
var less = require("gulp-less");

gulp.task("less", function () {
    return gulp.src('./wwwroot/css/styles.less')
            .pipe(less())
            .on('error', function (errorInfo) {
                console.log(errorInfo.toString());
                this.emit('end');
            })
            .pipe(gulp.dest('./wwwroot/css'));
});