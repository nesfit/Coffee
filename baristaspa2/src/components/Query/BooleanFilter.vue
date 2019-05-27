<template>
    <b-form-group :label="label" label-cols-sm="6" label-cols-md="4">
        <b-form-radio-group v-model="huh" :options="options" @input="updateValue" buttons button-variant="outline-info" />
    </b-form-group>
</template>

<script>
export default {
    name: "BooleanFilter",
    props: ['label','yes','no','idk','value'],
    data() {
        return {
            huh: "Null",
            options: [
                {text: this.yes && this.yes.length ? this.yes : "Yes", value: "True"},
                {text: this.no && this.no.length ? this.no : "No", value: "False"},
                {text: this.idk && this.idk.length ? this.idk : "Any", value: "Null"}
            ]
        }
    },
    methods: {
        updateValue(val) {
            this.$emit('input', val);
        }
    },
    mounted() {
        this.huh = this.value;
    },
    watch: {
        value() {
            this.huh = this.value;
        }
    }
}
</script>