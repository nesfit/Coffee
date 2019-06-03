import Vue from 'vue'
import App from './App'
import '@babel/polyfill'
import 'mutationobserver-shim'
import VueRouter from 'vue-router'
import './plugins/bootstrap-vue'
import axios from 'axios'
import Qs from 'qs'

import Home from '@/components/Pages/Home.vue'
import Users from '@/components/Pages/Users.vue'
import User from '@/components/Pages/User.vue'
import UserDetails from '@/components/Pages/UserDetails.vue'
import UserApiKeys from '@/components/Pages/UserApiKeys.vue'
import PasswordChange from '@/components/Pages/PasswordChange.vue'
import Message from '@/components/Message.vue'
import MyPurchases from '@/components/Pages/MyPurchases.vue'
import MyPayments from '@/components/Pages/MyPayments.vue'
import Products from '@/components/Pages/Products.vue'
import ProductDetails from "@/components/Pages/ProductDetails.vue"
import AccountingGroups from '@/components/Pages/AccountingGroups.vue'
import PointsOfSale from '@/components/Pages/PointsOfSale.vue'
import Sales from '@/components/Pages/Sales.vue'
import Payments from '@/components/Pages/Payments.vue'
import PointOfSale from '@/components/Pages/PointOfSale.vue'
import CardAssignment from '@/components/Pages/CardAssignment.vue'
import AccountingGroup from '@/components/Pages/AccountingGroup.vue'

import PointOfSaleDetails from '@/components/Pages/PointOfSaleDetails.vue'
import PointOfSaleSales from '@/components/Pages/PointOfSaleSales.vue'
import PointOfSaleOffers from '@/components/Pages/PointOfSaleOffers.vue'
import PointOfSaleStockItems from '@/components/Pages/PointOfSaleStockItems.vue'
import PointOfSaleAuthorizedUsers from '@/components/Pages/PointOfSaleAuthorizedUsers.vue'
import PointOfSaleIntegration from '@/components/Pages/PointOfSaleIntegration.vue'
import PointOfSaleStockOperations from '@/components/Pages/PointOfSaleStockOperations.vue'
import PointOfSaleApiKeys from '@/components/Pages/PointOfSaleApiKeys.vue';

import AccountingGroupDetails from "@/components/Pages/AccountingGroupDetails.vue"
import AccountingGroupAuthorizedUsers from "@/components/Pages/AccountingGroupAuthorizedUsers.vue"
import AccountingGroupPointsOfSale from "@/components/Pages/AccountingGroupPointsOfSale.vue"

Vue.use(VueRouter)
Vue.component("Message", Message)

import Paginate from 'vuejs-paginate'
Vue.component('Paginate', Paginate)

Vue.config.productionTip = false
Vue.prototype.$eventHub = new Vue();

const router = new VueRouter({
  base: __dirname,
  routes: [
    { path: '/', component: Home },
    { path: '/users', component: Users },
    { path: '/user/:id', component: User, children: [
      {path:'', component:UserDetails},
      {path:'apiKeys', component:UserApiKeys}
    ] },
    { path: '/home', component: Home },
    { path: '/passwordChange', component: PasswordChange },
    { path: '/my-purchases', component: MyPurchases },
    { path: '/my-payments', component: MyPayments },
    { path: '/products', component: Products },
    { path: '/products/:id', component: ProductDetails },
    { path: '/accountingGroups', component: AccountingGroups },
    { path: '/pointsOfSale', component: PointsOfSale },
    { path: '/sales', component: Sales },
    { path: '/payments', component: Payments },
    { path: '/pointOfSale/:id', component: PointOfSale, children: [
      {path:'', component:PointOfSaleDetails },
      {path:'sales', component:PointOfSaleSales},
      {path:'offers', component:PointOfSaleOffers},
      {path:'stockItems', component:PointOfSaleStockItems},
      {path:'authorizedUsers',component:PointOfSaleAuthorizedUsers},
      {path:'integration',component:PointOfSaleIntegration},
      {path:'stockOperations',component:PointOfSaleStockOperations},
      {path:'apiKeys',component:PointOfSaleApiKeys}
    ]},
    { path: '/cardAssignment', component: CardAssignment },
    { path: '/accountingGroups/:id', component: AccountingGroup, children: [
      {path:'', component:AccountingGroupDetails },
      {path:'pointsOfSale', component:AccountingGroupPointsOfSale },
      {path:'authorizedUsers', component:AccountingGroupAuthorizedUsers }
    ] }
  ]
})

function startApp() {
  /* eslint-disable no-new */
  new Vue({
    router,
    el: '#app',    
    template: '<App />',
    components: { App }
  })
}

var cfgLoader = axios.create({responseType: "json"});

var loadConfiguredBaseUrl = function() {
  cfgLoader.get("spa_config.json")
    .then(resp => {
      var axiosInstance = axios.create({
        responseType: "json",
        withCredentials: true,
        baseURL: resp.data.api_address,
        paramsSerializer: function (params) {
          return Qs.stringify(params, {arrayFormat: 'repeat'})
        }
      });

      Vue.prototype.$api = axiosInstance;

      startApp();
    })
    .catch(() => setTimeout(loadConfiguredBaseUrl, 1000));
};

loadConfiguredBaseUrl();

