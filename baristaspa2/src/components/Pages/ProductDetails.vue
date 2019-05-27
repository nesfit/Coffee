<template>
    <div v-if="id">
        <h1>Product Details</h1>

        <b-button @click="$router.go(-1)" class="mb-3">Back</b-button>

        <b-form @submit="editProduct">
            <b-form-group label="ID" label-for="id">
                <b-form-input readonly :value="id" />
            </b-form-group>
            
            <b-form-group label="Display name:" label-for="displayName">
                <b-form-input id="displayName" v-model="displayName" type="text" required />
            </b-form-group>

            <b-form-group label="Recommended price:" label-for="price">
                <b-form-input id="recommendedPrice" v-model="recommendedPrice" />
            </b-form-group>

            <b-button variant="danger" class="mr-2" v-bind:disabled="isDeleting" @click="deleteProduct">Delete Product</b-button> 
            <b-button type="submit" v-bind:disabled="isEditing || !displayName" variant="primary">Save</b-button>
        </b-form>
    </div>
</template>

<script>
export default {
    name: 'ProductDetails',
    data: function() {
        return {
            id: "",
            diplayName: "",
            recommendedPrice: "",
            isEditing: false,
            isDeleting: false
        };
    },
    methods: {
        editProduct: function(evt) {
            evt.preventDefault();
            
            var formData =  {
                displayName: this.displayName,
                recommendedPrice: this.recommendedPrice || null
            };

            var c = this;
            c.isEditing = true;

            c.$api.put("products/" + c.id, formData)
            .catch(c.$api.showError)
            .then(() => c.isEditing = false);
        },

        deleteProduct: function() {
            var c = this;

            this.$bvModal.msgBoxConfirm("Are you sure you want to delete user " + this.userToEdit.emailAddress + "?")
            .then(val => {
                if (!val) return;

                c.isDeleting = true;
                c.$api.delete("products/" + c.id)
                    .then(() => c.$router.push("/products")) 
                    .catch(c.$api.showError)
                    .then(() => c.isDeleting = false);
            });
        }
    },
    mounted() {
        var id = this.$route.params.id;
        var c = this;

        c.$api.get("products/" + id).then(resp => {
            c.id = id;
            c.displayName = resp.data.displayName;
            c.recommendedPrice = resp.data.recommendedPrice;
        }).catch(c.$api.showError);
    }
}
</script>