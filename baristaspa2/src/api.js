import axios from 'axios';

var apiport = 8081; // process.env.VUE_APP_API_PORT;
var baseurl = window.location.protocol + "//" + window.location.hostname + ":" + apiport + "/api/";

var api = axios.create({
    baseURL: baseurl,
    responseType: "json",
    withCredentials: true,
})

export default api;