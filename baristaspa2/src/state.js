import Api from "@/api.js"

var posNames = {};
var posPromises = {};

var agNames = {};
var agPromises = {};

var prodNames = {};
var prodPromises = {};

var userNames = {};
var userPromises = {};

var siNames = {};
var siPromises = {};

var ssNames = {};
var ssPromises = {};

export default {
    getPosName: function(posId) {
        if (posNames[posId] && (new Date).getTime() < posNames[posId].until)
            return new Promise((resolve) => resolve(posNames[posId].name));

        if (posPromises[posId])
            return new Promise((resolve, fail) => posPromises[posId].then(resolve).catch(fail));

        var p = new Promise(
            (resolve, reject) => Api.get("/pointsOfSale/" + posId)
                .then(response => {
                    posNames[posId] = { name: response.data.displayName, until: (new Date).getTime() + 30000 };
                    posPromises[posId] = null;
                    resolve(response.data.displayName);
                })
                .catch(() => {
                    posPromises[posId] = null;
                    reject();
                })
        );

        posPromises[posId] = p;
        return p;
    },

    getAgName: function(agId) {
        if (agNames[agId] && (new Date).getTime() < agNames[agId].until)
            return new Promise((resolve) => resolve(agNames[agId].name));

        if (agNames[agId])
            return new Promise((resolve, fail) => agPromises[agId].then(resolve).catch(fail));

        var p = new Promise(
            (resolve, reject) => Api.get("/accountingGroups/" + agId)
                .then(response => {
                    agNames[agId] = { name: response.data.displayName, until: (new Date).getTime() + 30000 };
                    agPromises[agId] = null;
                    resolve(response.data.displayName);
                })
                .catch(() => {
                    agPromises[agId] = null;
                    reject();
                })
        );

        agPromises[agId] = p;
        return p;
    },

    getProductName: function(prodId) {
        if (prodNames[prodId] && (new Date).getTime() < prodNames[prodId].until)
            return new Promise((resolve) => resolve(prodNames[prodId].name));

        if (prodNames[prodId])
            return new Promise((resolve, fail) => prodPromises[prodId].then(resolve).catch(fail));

        var p = new Promise(
            (resolve, reject) => Api.get("/products/" + prodId)
                .then(response => {
                    prodNames[prodId] = { name: response.data.displayName, until: (new Date).getTime() + 30000 };
                    prodPromises[prodId] = null;
                    resolve(response.data.displayName);
                })
                .catch(() => {
                    prodPromises[prodId] = null;
                    reject();
                })
        );

        prodPromises[prodId] = p;
        return p;
    },

    getUserName: function(userId) {
        if (userNames[userId] && (new Date).getTime() < userNames[userId].until)
            return new Promise((resolve) => resolve(userNames[userId].name));

        if (userPromises[userId])
            return new Promise((resolve, fail) => userPromises[userId].then(resolve).catch(fail));

        var p = new Promise(
            (resolve, reject) => Api.get("/users/" + userId)
                .then(response => {
                    userNames[userId] = { name: response.data.fullName, until: (new Date).getTime() + 30000 };
                    userPromises[userId] = null;
                    resolve(response.data.fullName);
                })
                .catch(() => {
                    userPromises[userId] = null;
                    reject();
                })
        );

        userPromises[userId] = p;
        return p;
    },

    getStockItemName: function(userId) {
        if (siNames[userId] && (new Date).getTime() < siNames[userId].until)
            return new Promise((resolve) => resolve(siNames[userId].name));

        if (siPromises[userId])
            return new Promise((resolve, fail) => siPromises[userId].then(resolve).catch(fail));

        var p = new Promise(
            (resolve, reject) => Api.get("/stockItems/" + userId)
                .then(response => {
                    siNames[userId] = { name: response.data.displayName, until: (new Date).getTime() + 30000 };
                    siPromises[userId] = null;
                    resolve(response.data.displayName);
                })
                .catch(() => {
                    siPromises[userId] = null;
                    reject();
                })
        );

        siPromises[userId] = p;
        return p;
    },

    getSsName: function(ssId) {
        if (ssNames[ssId] && (new Date).getTime() < ssNames[ssId].until)
            return new Promise((resolve) => resolve(ssNames[ssId].name));

        if (ssPromises[ssId])
            return new Promise((resolve, fail) => ssPromises[ssId].then(resolve).catch(fail));

        var p = new Promise(
            (resolve, reject) => Api.get("/saleStrategies/" + ssId)
                .then(response => {
                    ssNames[ssId] = { name: response.data.displayName, until: (new Date).getTime() + 30000 };
                    ssPromises[ssId] = null;
                    resolve(response.data.displayName);
                })
                .catch(() => {
                    ssPromises[ssId] = null;
                    reject();
                })
        );

        ssPromises[ssId] = p;
        return p;
    }
}