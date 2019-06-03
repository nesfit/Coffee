<template>
    <div v-if="posId">
      <h2>Reported key-values</h2>
      <b-table primary-key="key" striped hover :items="items" :fields="fields" />

      <h2>Commands</h2>
      <b-button
        v-for="cmdName in availableCommands"
        :key="cmdName"
        @click="sendCommand(cmdName)">
        {{cmdName}}
      </b-button>

      <div v-if="canDispenseProduct">
        <h2>Product Dispensal</h2>
        <b-form>
          <ProductSelector label="Product" v-bind:required="true" v-model="productIdToDispense" />
          <b-button variant="primary" @click="dispenseProduct" v-bind:disabled="!productIdToDispense">Dispense</b-button>
        </b-form>      
      </div>
    </div>
</template>

<script>
import ProductSelector from '@/components/Selection/ProductSelector.vue'

export default {
  name: 'PointOfSaleIntegration',
  components: {ProductSelector},
  computed: {
    items: function() {
      if (!this.posData || !this.posData.keyValues)
        return [];

      var items = [];
      var keys = Object.keys(this.posData.keyValues);
      for (var i = 0; i < keys.length; i++)
        items.push({ key: keys[i], value: this.posData.keyValues[keys[i]] });
      
      return items;
    },

    featureMap: function() {
      if (!this.posData || !this.posData.features)
        return {};

      var features = {};
      for (var i = 0; i < this.posData.features.length; i++)
        features[this.posData.features[i]] = true;
      
      return features;
    },

    availableCommands: function() {
      var cmds = [];
      var allCmds = this.commandsWithFeatureRequirements;

      for (var cmdName in allCmds)
        if (this.featureMap[allCmds[cmdName]])
          if (cmdName != "dispenseProduct")
            cmds.push(cmdName);

      return cmds;
    },

    canDispenseProduct: function() {
      return this.featureMap["ProductDispenser"];
    }
  },
  methods: {
    sendCommand: function(cmdName) {
      switch (cmdName) {
        case "powerOn":
        case "powerOff":
        case "startCleaning":
          this.sendCommandInner("pointsOfSale/" + this.posData.id + "/command/" + cmdName);
          break;
      }
    },

    dispenseProduct(product) {
      this.sendCommandInner("pointsOfSale/" + this.posData.id + "/command/dispenseProduct", { productId: product.id });
    },

    sendCommandInner(path, args) {
      var c = this;
      c.$api.post(path, args)
        .then(() => c.$bvModal.msgBoxOk("The API accepted the request to relay the command."))
        .catch(e => c.$api.showError(e, "The API rejected to request to relay the command."));
    }
  },
  data: function() {
    return {
      posId: null,
      posData: null,
      productIdToDispense: "",
      fields: [
        {
          key: "key",
          label: "Key",
          sortable: true
        },
        {
          key: "value",
          label: "Value",
          sortable: true
        }
      ],

      commandsWithFeatureRequirements: {
        "dispenseProduct": "ProductDispenser",
        "powerOff": "PowerStateManagement",
        "powerOn": "PowerStateManagement",
        "startCleaning": "CleaningInitiator"
      }
    };
  },
  mounted() {
    var c = this;
    c.$api.get("pointsOfSale/" + this.$route.params.id).then(response => {
      c.posId = c.$route.params.id;
      c.posData = response.data;
    });
  }
}
</script>