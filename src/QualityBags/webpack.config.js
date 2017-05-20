var path = require("path");

module.exports = {
    entry: "./wwwroot/js/site.js",
    output: {
        path: path.resolve("wwwroot/js"),
        filename: "scripts.js"
    },
    module: {
        loaders: [
			{
                loader: 'babel-loader',
                query: {
                    presets: ['es2015']
                },
                test: /\.js$/,
                exclude: /(node_modules|Gulp|wwwroot\/lib)/
			}
        ]
    }
};