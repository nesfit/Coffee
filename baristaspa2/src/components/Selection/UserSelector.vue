<template>
    <div>
        <b-input-group>
            <b-form-input v-bind:class="(!this.selectedName || !this.selectedName.length) ? 'noSelection' : ''" readonly v-bind:value="selectedName" />

            <b-input-group-append>
                <b-button variant="outline-primary" @click="selectClicked">Select</b-button>
                <b-button v-if="!this.required && (this.value && this.value.length > 0)" variant="outline-danger" @click="unsetClicked">Unset</b-button>
            </b-input-group-append>
        </b-input-group>

        <b-modal size="lg" ref="modal" hide-footer title="User Selection">
            <b-form>
                <TextFilter label="Full Name" v-model="fullName" />
                <TextFilter label="E-mail Address" v-model="emailAddress" />
                <BooleanFilter label="Is Administrator" v-model="isAdministrator" />
                <BooleanFilter label="Is Active" v-model="isActive" />
            </b-form>

            <PagedQuery v-bind:fields="fields" endpoint="users/details" startingSortBy="fullName" startingSortDir="desc"
                v-bind:additionalQueryParams="additionalParams" @item-clicked="onSelected" />
        </b-modal>
    </div>
</template>

<script>
import PagedQuery from '@/components/Query/PagedQuery.vue'
import TextFilter from '@/components/Query/TextFilter.vue'
import BooleanFilter from '@/components/Query/BooleanFilter.vue'
import State from '@/state.js';

export default {
    name: 'UserSelector',
    components: { PagedQuery, TextFilter, BooleanFilter },
    props: {
        value: String,
        required: Boolean
    },
    data() {
        return {
            selectedName: "Initializing..",
            emailAddress: "",
            isAdministrator: "Null",
            fullName: "",
            isActive: "Null",
            fields: [
                {
                    key: "fullName",
                    label: "Full Name",
                    sortable: true
                },
                {
                    key: "emailAddress",
                    label: "E-mail address",
                    sortable: true
                },
                {
                    key: "isAdministrator",
                    label: "Is Administrator?",
                    sortable: true
                },
                {
                    key: "isActive",
                    label: "Is Active?",
                    sortable: true
                }
            ]
        }
    },
    computed: {
        additionalParams() {
            return {
                emailAddress: this.emailAddress.length > 0 ? this.emailAddress : null,
                fullName: this.fullName.length > 0 ? this.fullName : null,
                isAdministrator: this.isAdministrator == "Null" ? null : (this.isAdministrator == "True"),
                isActive: this.isActive == "Null" ? null : (this.isActive == "True")
            };
        }
    },
    methods: {
        updateDisplay() {
            if (!this.value || !this.value.length)
                this.selectedName = "none";
            else {
                this.selectedName = "Loading..";
                var c = this;
                State.getUserName(c.$api, c.value)
                    .then(n => c.selectedName = n)
                    .catch(() => c.selectedName = "Unknown");
            }

            this.$emit("selection-changed");
        },
        selectClicked() {
            this.$refs.modal.show();
        },
        unsetClicked() {
            this.$emit("input", "");
        },
        onSelected(item) {
            this.$emit("input", item.id);
            this.$refs.modal.hide();
        }
    },
    mounted() {
        this.updateDisplay();
    },
    watch: {
        value() {
            this.updateDisplay();
        }
    }
}

</script>

<style scoped>
.noSelection {
    font-style: italic;
}
</style>