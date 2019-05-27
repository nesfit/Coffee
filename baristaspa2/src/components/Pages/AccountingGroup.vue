<template>
    <div v-if="agId">
        <h1>{{ agData.displayName || "Loading.." }}</h1>

        <div class="container">
            <div class="row">
                <b-nav pills vertical class="col-sm-12 col-md-4">
                    <b-nav-item active-class="active" :to="'/accountingGroups/'+this.agId+'/'" exact>Details</b-nav-item>
                    <b-nav-item active-class="active" :to="'/accountingGroups/'+this.agId+'/pointsOfSale'">Points of Sale</b-nav-item>
                    <b-nav-item active-class="active" :to="'/accountingGroups/'+this.agId+'/authorizedUsers'">Authorized Users</b-nav-item>
                </b-nav>

                <router-view class="col-sm-12 col-md-8"></router-view>
            </div>
        </div>
    </div>
</template>

<script>

import Api from '@/api.js'

export default {
    name: 'AccountingGroup',
    data: function() {
        return {
            agId: null,
            agData: {}
        }
    },
    created() {
        this.$eventHub.$on('ag-renamed', this.agRenamed);
    },
    beforeDestroy() {
        this.$eventHub.$off('ag-renamed');
    },
    methods: {
        loadAg() {
            var c = this;

            Api.get("accountingGroups/" + this.agId).then(response => {
                c.agData = response.data;
            });
        },

        agRenamed(id, displayName) {
            if (id == this.agId)
                this.agData.displayName = displayName;
        }
    },
    mounted: function() {
        this.agId = this.$route.params.id;
        this.loadAg();
    }
}
</script>