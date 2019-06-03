<template>
    <div>
        <b-modal :id="modalId" title="Authorized User Details" hide-footer>
            <b-form>
                <b-form-group label="User">
                    <UserName v-if="userId" v-bind:id="userId" />
                </b-form-group>

                <b-form-group label="Level">
                    <b-form-select v-model="form.level" class="mb-3">
                        <option value="AuthorizedUser">Authorized user</option>
                        <option value="Owner">Owner</option>
                    </b-form-select>
                </b-form-group>

                <b-button type="submit" @click="updateAuthorization">Update Authorization</b-button>
                <b-button type="submit" @click="revokeAuthorization" variant="danger">Revoke Authorization</b-button>
            </b-form>
        </b-modal>
    </div>
</template>

<script>
import UserName from "@/components/Display/UserName.vue";

export default {
    name: 'PosAuthorizedUserDetailsModal',
    components: {UserName},
    props: {
        modalId: String,
        posId: String,
        values: Object
    },
    data: function() {
        return {
            userId: "",
            form: {
                "level": "AuthorizedUser",
            }
        };
    },
    watch: {
        values() {
            this.form.level = this.values.level;
            this.userId = this.values.userId;
        }
    },
    methods: {
        updateAuthorization: function(evt) {
            evt.preventDefault();
            var c = this;

            c.$api.put("pointsOfSale/" + c.posId + "/authorizedUsers/" + c.userId, c.form)
                .then(() => c.$bvModal.msgBoxOk("User authorization updated"))
                .catch(() => c.$bvModal.msgBoxOk("User authorization failed"));
        },

        revokeAuthorization: function(evt) {
            evt.preventDefault();
            var c = this;

            c.$api.delete("pointsOfSale/" + c.posId + "/authorizedUsers/" + c.userId)
                .then(() => {
                    c.$bvModal.msgBoxOk("User authorization revoked");
                    c.$bvModal.hide(this.modalId);
                })
                .catch(e => c.$api.showError(e, "User authorization revocation failed"));
        }
    }
}
</script>