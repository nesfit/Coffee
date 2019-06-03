<template>
    <b-modal size="md" v-if="posId" ref="modal" title="Create New API Key" ok="Create" @hidden="onCancel" @close="onCancel" @cancel="onCancel" v-bind:ok-disabled="!keyCreated && (isCreating || !label)" @ok="onSubmit">
        <b-form-group label="Label:" label-for="label" v-if="!keyCreated">
            <b-form-input id="label" v-model="label" type="text" required />
        </b-form-group>

        <p v-if="keyCreated">Please copy the key from the box below and store it somewhere safe. The key cannot be displayed again.</p>
        
        <b-form-group label="API key" label-for="apiKey" v-if="keyCreated">
            <b-textarea readonly v-bind:value="createdApiKey" />
        </b-form-group>
    </b-modal>
</template>

<script>
export default {
    name: 'PosApiKeyCreationModal',
    props: {
        posId: String
    },
    data: function() {
        return {
            keyCreated: false,
            createdApiKey: "",
            label: "",
            isCreating: false
        };
    },
    methods: {
        show: function() {
            if (this.posId)
                this.$refs.modal.show();
        },
        onCancel: function() {
            if (this.keyCreated) {
                this.keyCreated = false;
                this.createdApiKey = "";
                this.label = "";
            }
        },
        onSubmit: function(evt) {
            if (this.keyCreated) {
                this.onCancel();
                return;
            }

            evt.preventDefault();

            var c = this;
            c.isCreating = true;
            var formData = { label: c.label };

            c.$api.post("pointsOfSale/"+this.posId+"/apiKeys", formData)
                .then(resp => {
                    c.createdApiKey = resp.data.value;
                    c.keyCreated = true;
                    c.$emit("api-key-created");
                })
                .catch(e => c.$api.showError(e, "POS API key creation failed"))
                .then(() => c.isCreating = false);
        }
    }
}
</script>