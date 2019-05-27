<template>
    <div>
        <h1>{{ posData.displayName || "Loading.." }}</h1>
        
        <div class="container">
            <div class="row">
                <b-nav pills vertical class="col-sm-12 col-md-4">
                    <b-nav-item active-class="active" :to="'/pointOfSale/'+this.posId+'/'" exact>Details</b-nav-item>
                    <b-nav-item active-class="active" :to="'/pointOfSale/'+this.posId+'/sales'">Sales</b-nav-item>
                    <b-nav-item active-class="active" :to="'/pointOfSale/'+this.posId+'/offers'">Offers</b-nav-item>
                    <b-nav-item active-class="active" :to="'/pointOfSale/'+this.posId+'/stockItems'">Stock Items</b-nav-item>
                    <b-nav-item active-class="active" :to="'/pointOfSale/'+this.posId+'/stockOperations'">Stock Operations</b-nav-item>
                    <b-nav-item active-class="active" :to="'/pointOfSale/'+this.posId+'/authorizedUsers'">Authorized Users</b-nav-item>
                    <b-nav-item active-class="active" :to="'/pointOfSale/'+this.posId+'/integration'">Integration</b-nav-item>
                    <b-nav-item active-class="active" :to="'/pointOfSale/'+this.posId+'/apiKeys'">API keys</b-nav-item>
                </b-nav>

                <router-view class="col-sm-12 col-md-8"></router-view>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: 'PointOfSale',
    data: function() {
        return {
            posId: null,
            posData: {}
        };
    },
    methods: {
        loadPointOfSale() {
            var c = this;

            c.$api.get("pointsOfSale/" + this.posId).then(response => {
                c.posData = response.data;
            });
        },
        posRenamed(id, displayName) {
            if (this.posId == id)
                this.posData.displayName = displayName;
        }
    },
    created() {
        this.$eventHub.$on('pos-renamed', this.posRenamed);
    },
    beforeDestroy() {
        this.$eventHub.$off('pos-renamed');
    },
    mounted: function() {
        this.posId = this.$route.params.id;
        this.loadPointOfSale();
    }
}
</script>

<style>
</style>