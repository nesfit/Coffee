<template>
    <b-form-group :label="label" label-cols-sm="6" label-cols-md="4">
        <b-input-group>
            <b-form-input ref="input" :type="formInputType" v-model="formInputContent" @input="updateValue" />
            <b-input-group-append>
                <b-button variant="outline-info" @click="resetValue">Reset</b-button>
            </b-input-group-append>
        </b-input-group>
    </b-form-group>
</template>

<script>
export default {
    name: "TextFilter",
    props: {
        value: String,
        label: String,
        inputType: String
    },
    data() {
        return { formInputContent: this.value, formInputType: this.inputType && this.inputType.length ? this.inputType : "text" }
    },
    methods: {
        updateValue(newVal) {
           this.$emit('input', newVal);
        },
        resetValue() {
            this.$emit('input', '');
        }
    },
    watch: {
        value() {
            this.formInputContent = this.value;
        }
    },
    mounted() {
        this.formInputContent = this.value;
    }
}
</script>