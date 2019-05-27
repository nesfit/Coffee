<template>
    <div>
        <b-input-group>
            <b-form-input v-bind:class="(!this.selectedName || !this.selectedName.length) ? 'noSelection' : ''" readonly v-bind:value="selectedName" />

            <b-input-group-append>
                <b-button variant="outline-primary" @click="selectClicked">Select</b-button>
                <b-button v-if="!this.required && (this.value && this.value.length > 0)" variant="outline-danger" @click="unsetClicked">Unset</b-button>
            </b-input-group-append>
        </b-input-group>

        <b-modal ref="modal" hide-footer title="Accounting Group Selection" size="lg">
            <b-form>
                <TextFilter label="Display Name" v-model="displayName" />
            </b-form>

            <PagedQuery v-bind:fields="fields" endpoint="accountingGroups" @item-clicked="onSelected"
                startingSortBy="displayName" startingSortDir="desc" v-bind:additionalQueryParams="additionalParams" />
        </b-modal>
    </div>
</template>

<script>
import PagedQuery from '@/components/Query/PagedQuery.vue'
import TextFilter from '@/components/Query/TextFilter.vue'
import State from '@/state.js';

export default {
    name: 'AgSelector',
    components: { PagedQuery, TextFilter },
    props: {
        value: String,
        required: Boolean
    },
    data() {
        return {
            selectedName: "Initializing..",
            displayName: "",
            fields: [
                {
                    key: "displayName",
                    label: "Display Name"
                }
            ]
        }
    },
    computed: {
        additionalParams() {
          return { displayName: this.displayName };
        }
    },
    methods: {
        updateDisplay() {
            if (!this.value || !this.value.length)
                this.selectedName = "none";
            else {
                this.selectedName = "Loading..";
                var c = this;
                State.getAgName(c.value)
                    .then(n => c.selectedName = n)
                    .catch(() => c.selectedName = "Unknown");
            }
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