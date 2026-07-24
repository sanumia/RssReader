// const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const Dotenv = require('dotenv-webpack');

const pkg  = require("./package.json");
const commonPaths = require("./build_utils/config/commonPaths");

const isDebug = !process.argv.includes("release");

const port = process.env.PORT || 3000;

module.exports = {
    entry: commonPaths.entryPath,
    output: {
        uniqueName: pkg.name,
        publicPath: "/",
        path: commonPaths.outputPath,
        filename: `${pkg.version}/js/[name].[chunkhash:8].js`,
        chunkFilename: `${pkg.version}/js/[name].[chunkhash:8].js`,
        assetModuleFilename: isDebug
        ? `images/[path][name].[contenthash:8][ext]`
        : `images/[path][contenthash:8][ext]`,
        crossOriginLoading: "anonymous",
    },
    plugins: [
        new Dotenv(),
        new HtmlWebpackPlugin({
        template: "public/index.html",
        filename: "index.html",
        })
    ],
    devServer: {
        port: port,
        static: {
        directory: commonPaths.outputPath,
        },
        historyApiFallback: {
        index: "index.html",
        },
        webSocketServer: false,
    },
    module: {
        rules: [
        {
            test: /\.(js|jsx)$/,
            exclude: /node_modules/, // exclude node_modules
            use: ["babel-loader"],
        },
        ],
    },
    resolve: {
        extensions: ["*", ".js", ".jsx"],
    },
};