<template>
    <div v-if="agId">
        <b-form>
            <TextFilter v-model="posDisplayName" label="Display Name" /> 
        </b-form>

        <PagedQuery v-bind:fields="fields" endpoint="pointsOfSale" @item-clicked="posClicked" v-bind:additionalQueryParams="additionalParams" />
    </div>
</template>

<script>
import TextFilter from "@/components/Query/TextFilter.vue"
import PagedQuery from '@/components/Query/PagedQuery.vue'

export default {
    name: 'AccountingGroupPointsOfSale',
    components: { PagedQuery, TextFilter },
    data() {
        return {
            agId: null,
            posDisplayName: "",
            fields: [
                {
                    key: "displayName",
                    label: "DisplayName"
                },
                {
                    key: "id",
                    label: "ID"
                }
            ]
        }
    },
    mounted() {
        this.agId = this.$route.params.id;
    },
    computed: {
        additionalParams() {
            var params = {parentAccountingGroupId: this.agId};
            if (this.posDisplayName)
                params.displayName = this.posDisplayName;

            return params;
        }
    },
    methods: {
        posClicked(row) {
            this.$route.push("/pointsOfSale/" + row.id);
        }
    }
}

</script>