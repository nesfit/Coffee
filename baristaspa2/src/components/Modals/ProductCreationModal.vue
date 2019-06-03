<template>
    <b-modal ref="modal" title="Create New Product" ok="Create Product" v-bind:ok-disabled="isCreating || !displayName" @ok="onSubmit">
        <b-form-group label="Display name:" label-for="displayName">
            <b-form-input id="displayName" v-model="displayName" type="text" required />
        </b-form-group>

        <b-form-group label="Recommended price:" label-for="price">
            <b-form-input id="recommendedPrice" v-model="recommendedPrice" />
        </b-form-group>
    </b-modal>
</template>

<script>
export default {
    name: 'ProductCreationModal',
    data: function() {
        return {
            displayName: "",
            recommendedPrice: "",
            isCreating: false
        };
    },
    methods: {
        show: function() {
            this.$refs.modal.show();
        },
        onSubmit: function() {
            var c = this;
            c.isCreating = true;
            var formData = { displayName: c.displayName, recommendedPrice: c.recommendedPrice || null };

            c.$api.post("products", formData)
                .then(resp => c.$router.push("products/" + resp.data.id))
                .catch(err => {
                    c.$api.showError(err, "Product creation failed");
                    c.isCreating = false;
                });
        }
    }
}
</script>