<template>
    <b-modal ref="modal" title="Create New User" ok="Create User" v-bind:ok-disabled="isCreating || !emailAddress || !fullName" @ok="onSubmit">
        <b-form>
            <b-form-group label="E-mail Address" label-for="email">
                <b-form-input id="email" v-model="emailAddress" type="email" required />
            </b-form-group>

            <b-form-group label="Full Name" label-for="fullName">
                <b-form-input id="fullName" v-model="fullName" type="text" required />
            </b-form-group>

            <b-form-group label="Is Administrator" label-for="isAdmin">
                <b-form-checkbox id="isAdmin" v-model="isAdministrator" />
            </b-form-group>

            <b-form-group label="Is Active" label-for="isActive">
                <b-form-checkbox id="isActive" v-model="isActive" />
            </b-form-group>
        </b-form>
    </b-modal>
</template>

<script>
export default {
    name: 'UserCreateForm',
    data: function() {
        return {
            isCreating: false,
            emailAddress: "",
            fullName: "",
            isAdministrator: false,
            isActive: true
        };
    },
    methods: {
        onSubmit(evt) {
            evt.preventDefault();
            this.isCreating = true;
            var c = this;

            var form = {
                emailAddress: c.emailAddress,
                fullName: c.fullName,
                isAdministrator: c.isAdministrator,
                isActive: c.isActive
            };

            c.$api.post("users", form)
                .then(resp => {
                    c.$refs.modal.hide();
                    c.$emit("user-created", resp.data.id);                    
                })
                .catch(e => c.$api.showError(e, "User creation failed"))
                .then(() => this.isCreating = false);
        },

        show() {
            this.$refs.modal.show();
        }
    }
}
</script>