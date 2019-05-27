import axios from 'axios';

var apiport = 8081; // default
var baseurl = window.location.protocol + "//" + window.location.hostname + ":" + apiport + "/api/";

var api = axios.create({
    baseURL: baseurl,
    responseType: "json",
    withCredentials: true,
});

var cfgLoader = axios.create({responseType: "json"});

function loadConfiguredBaseUrl() {
	cfgLoader.get("spa_config")
		.then(resp => { api.defaults.baseURL = resp.data.api_address })
		.catch(() => setTimeout(loadConfiguredBaseUrl, 3000));
}

loadConfiguredBaseUrl();

export default api;
