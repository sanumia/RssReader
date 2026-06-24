import js from "@eslint/js";
import globals from "globals";
import pluginReact from "eslint-plugin-react";

export default [
    js.configs.recommended,

    {
        files: ["src/**/*.{js,jsx}"],
        languageOptions: {
        ecmaVersion: "latest",
        sourceType: "module",
        globals: {
            ...globals.browser,
        },
        parserOptions: {
            ecmaFeatures: { jsx: true },
        },
        },
        plugins: {
        react: pluginReact,
        },
        rules: {
        ...pluginReact.configs.flat.recommended.rules,
        "react/react-in-jsx-scope": "off",
        "react/jsx-uses-react": "off",
        },
        settings: {
        react: {
            version: "detect",
        },
        },
    },

    {
        files: ["webpack.config.js", "build_utils/**/*.js"],
        languageOptions: {
        ecmaVersion: "latest",
        sourceType: "commonjs",
        globals: {
            ...globals.node,
        },
        },
    },

    // Ignore output/build artifacts and dependencies entirely
    {
        ignores: ["build/**", "node_modules/**", "dist/**"],
    },
];