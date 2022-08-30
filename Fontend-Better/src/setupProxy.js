const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function (app) {
    app.use("/api/**", createProxyMiddleware({
        target: 'https://172.73.72.33:49153',
        changeOrigin: true,
        secure: false
    }));
};
