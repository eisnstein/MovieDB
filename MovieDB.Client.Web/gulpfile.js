const { src, dest, watch } = require("gulp");
const postCss = require("gulp-postcss");
const cleanCss = require("gulp-clean-css");
const rename = require("gulp-rename");

function development(cb) {
  return src("./Styles/style.css")
    .pipe(postCss([require("postcss-import"), require("tailwindcss"), require("autoprefixer")]))
    .pipe(dest("./wwwroot/css"));
}

function production(db) {
  return src("./Styles/style.scss")
    .pipe(postCss([require("postcss-import"), require("tailwindcss"), require("autoprefixer")]))
    .pipe(cleanCss())
    .pipe(rename({ extname: ".min.css" }))
    .pipe(dest("./wwwroot/css"));
}

exports.default = production;
exports.dev = function() {
  watch("./Styles/**/*.css", development);
};
