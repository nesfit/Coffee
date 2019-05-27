<template>
    <div>
      <p v-if="loadingUserBalance">Loading your current balance..</p>
      <p v-else-if="userBalanceValue === null">An error occurred while loading your balance.</p>
      <p v-else>Your balance is {{ userBalanceValue }} as of {{ userBalanceAsOf }}.</p>
    </div>

    
</template>

<script>

// import PagedQuery from '@/components/Query/PagedQuery.vue'

export default {
  name: 'Home',
  // components: {PagedQuery},
  data: function() {
    return {
      userBalanceValue: null,
      userBalanceAsOf: null,
      loadingUserBalance: true,
      fields: {
        
      }
    };
  },
  mounted: function() {
    var c = this;

    c.$api.get("balance/me")
      .then(response => {
        this.userBalanceValue = response.data.value;
        this.userBalanceAsOf = response.data.asOf;
      })
      .then(() => this.loadingUserBalance = false);
  }
}
</script>