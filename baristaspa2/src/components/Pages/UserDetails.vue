<template>
    <div v-if="id">
        <b-modal ref="passwordModal" title="Set Password">
            <b-form @submit="setPassword">
                <b-form-group label="New password (visible):" label-for="newPass">
                    <b-form-input id="newPass" v-model="newPasswordToAssign" type="text" required />
                </b-form-group>

                <b-button type="submit" v-bind:disabled="!newPasswordToAssign || isSettingPassword">Set User Password</b-button>
            </b-form>

            <template slot="modal-footer">&nbsp;</template>
        </b-modal>

        <b-form @submit="onSubmit">
            <b-form-group label="ID" label-for="id">
                <b-form-input readonly :value="id" />
            </b-form-group>

            <b-form-group label="E-mail Address" label-for="email">
                <b-form-input id="email" v-model="emailAddress" type="email" required />
            </b-form-group>

            <b-form-group label="Display Name" label-for="displayName">
                <b-form-input id="displayName" v-model="fullName" type="text" required />
            </b-form-group>

            <b-form-group label="Balance" label-for="balance">
                <b-form-input readonly v-bind:value="balance" />
            </b-form-group>

            <b-form-group label="Is Administrator" label-for="isAdmin">
                <b-form-checkbox id="isAdmin" v-model="isAdministrator" />
            </b-form-group>

            <b-form-group label="Is Active" label-for="isActive">
                <b-form-checkbox id="isActive" v-model="isActive" />
            </b-form-group>

            <b-button variant="danger" @click="deleteUser" class="mr-2">Delete User</b-button>
            <b-button @click="$refs.passwordModal.show()" class="mr-2">Set Password</b-button>
            <b-button variant="primary" type="submit" v-bind:disabled="isSaving || !emailAddress || !fullName">Edit User</b-button>
        </b-form>
    </div>
</template>

<script>
export default {
    name: 'UserDetails',
    data: function() {
        return {
            id: null,
            isSaving: false,
            isSettingPassword: false,

            emailAddress: "",
            fullName: "",
            isAdministrator: false,
            isActive: false,
            newPasswordToAssign: "",

            balance: "Loading.."
        };
    },
    mounted() {
        var c = this;
        var id = c.$route.params.id;

        c.$api.get("users/" + id + "/details")
            .then(resp => {
                c.id = id;
                c.emailAddress = resp.data.emailAddress;
                c.fullName = resp.data.fullName;
                c.isAdministrator = resp.data.isAdministrator;
                c.isActive = resp.data.isActive;
            })
            .catch(c.$api.showError);

        c.$api.get("balance/" + id)
            .then(resp => c.balance = resp.data.value)
            .catch(c.$api.showError);
    },
    methods: {
        onSubmit: function(evt) {
            evt.preventDefault();
            var c = this;
            c.isSaving = true;

            var formData = {
                emailAddress: c.emailAddress,
                fullName: c.fullName,
                isAdministrator: c.isAdministrator,
                isActive: c.isActive
            };

            c.$api.put("users/"+c.id, formData)
            .then(() => {
                c.$eventHub.$emit('user-renamed', c.id, c.fullName);
            })
            .catch(c.$api.showError)
            .then(() => c.isSaving = false);
        },

        deleteUser(evt) {
            evt.preventDefault();
            var c = this;

            this.$bvModal.msgBoxConfirm("Are you sure you want to delete this user?")
            .then(val => {
                if (!val) return;
                    c.$api.delete("users/" + this.id)
                    .then(() => c.$router.push("/users"))
                    .catch(e => c.$api.showError(e, "User deletion failed"))
            });
        },

        setPassword(evt) {
            var c = this;
            evt.preventDefault();

            c.isSettingPassword = true;

            c.$api.put("users/" + this.id + "/password", { newPassword: this.newPasswordToAssign })
            .then(() => {
                c.$refs.passwordModal.hide();
                c.newPasswordToAssign = "";
            })
            .catch(e => c.$api.showError(e, "Password override failed."))
            .then(() => c.isSettingPassword = false);
        }
    }
}
</script>