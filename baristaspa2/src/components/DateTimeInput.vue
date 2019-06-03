<template>
    <div>
        <b-form-input v-if="this.required" v-model="inputValue" @input="publishInputValue" :placeholder="placeholder" type="datetime-local" required />
        
        <b-input-group v-else>
            <b-button v-if="!this.inputValue || this.inputValue.length == 0" variant="outline-success" @click="set">Set Value</b-button>

            <b-form-input v-if="this.inputValue && this.inputValue.length > 0" v-model="inputValue" @input="publishInputValue" :placeholder="placeholder" type="datetime-local" />
            <b-input-group-append v-if="this.inputValue && this.inputValue.length > 0">
                <b-button variant="outline-danger" @click="unset">Unset Value</b-button>
            </b-input-group-append>
        </b-input-group>
    </div>
</template>

<script>
export default {
    name: "DateTimeInput",
    props: {
        value: String,
        placeholder: String,
        required: Boolean
    },
    data() {
        return { inputValue: this.toDatetimeLocal(new Date()) };
    },
    mounted() {
        this.updateInput();
    },
    watch: {
        value() {
            this.updateInput();
        }
    },
    methods: {
        unset(evt) {
            evt.preventDefault();
            this.$emit("input", "");
        },
        set(evt) {
            evt.preventDefault();
            this.$emit("input", new Date().toISOString());
        },
        updateInput() {    
            if (this.value && this.value.length)
                this.inputValue = this.toDatetimeLocal(new Date(this.value));
            else
                this.inputValue = "";
        },
        publishInputValue(val) {
            var valDate = new Date(val);
            this.$emit("input", valDate.toISOString());
        },
        toDatetimeLocal(date) {
            var twoDigits = function (i) {
                return (i < 10 ? '0' : '') + i;
            };

            var year = date.getFullYear();
            var month = twoDigits(date.getMonth() + 1);
            var day = twoDigits(date.getDate());

            var hrs = twoDigits(date.getHours());
            var mins = twoDigits(date.getMinutes());
            
            return year + '-' + month + '-' + day + 'T' + hrs + ':' + mins;
        }
    }
};
</script>