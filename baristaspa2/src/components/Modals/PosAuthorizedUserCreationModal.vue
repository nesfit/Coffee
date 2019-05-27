<template>
    <div>
        <b-modal :id="modalId" title="New Authorized User" hide-footer>
            <b-form>
                <UserSelector v-model="userId" label="User" v-bind:required="true" />

                <b-form-group label="Level">
                    <b-form-select v-model="level" class="mb-3">
                        <option value="AuthorizedUser">Authorized user</option>
                        <option value="Owner">Owner</option>
                    </b-form-select>
                </b-form-group>

                <b-button type="submit" @click="authorizeUser" v-bind:disabled="!userId">Authorize User</b-button>
            </b-form>
        </b-modal>
    </div>
</template>

<script>
import Api from '@/api.js';
import UserSelector from "@/components/Selection/UserSelector.vue";

export default {
    name: 'PosAuthorizedUserCreationModal',
    components: {UserSelector},
    props: {
        modalId: String,
        posId: String
    },
    data: function() {
        return {
            userId: "",
            level: "AuthorizedUser"
        };
    },
    methods: {
        userSelected: function(user) {
            this.userId = user.id;
        },

        authorizeUser: function(evt) {
            evt.preventDefault();
            var c = this;

            Api.post("pointsOfSale/" + c.posId + "/authorizedUsers/" + c.userId, { level: c.level })
                .then(() => {
                    c.$emit("user-authorized", c.userId);
                    c.$bvModal.hide(c.modalId);
                })
                .catch(() => c.$bvModal.msgBoxOk("User authorization failed"));
        }
    }
}
</script>