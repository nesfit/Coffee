<template>
    <div>
        <h1>{{ userData.fullName }}</h1>
        
        <div class="container">
            <div class="row">
                <b-nav pills vertical class="col-sm-12 col-md-4">
                    <b-nav-item active-class="active" :to="'/user/'+this.userId+'/'" exact>Details</b-nav-item>
                    <b-nav-item active-class="active" :to="'/user/'+this.userId+'/apiKeys'">API keys</b-nav-item>
                </b-nav>

                <router-view class="col-sm-12 col-md-8"></router-view>
            </div>
        </div>
    </div>
</template>

<script>

import Api from '@/api.js'

export default {
    name: 'User',
    data: function() {
        return {
            userId: null,
            userData: {}
        };
    },
    methods: {
        loadUser() {
            var c = this;

            Api.get("users/" + this.userId).then(response => {
                c.userData = response.data;
            });
        },
        userRenamed(id, fullName) {
            if (this.userId == id)
                this.userData.fullName = fullName;
        }
    },
    created() {
        this.$eventHub.$on('user-renamed', this.userRenamed);
    },
    beforeDestroy() {
        this.$eventHub.$off('user-renamed');
    },
    mounted: function() {
        this.userId = this.$route.params.id;
        this.loadUser();
    }
}
</script>