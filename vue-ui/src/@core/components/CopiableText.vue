<script setup lang='ts'>
import { ref } from 'vue'

const props = defineProps({
  value: {
    type: String,
    required: true,
  },
  label: {
    type: String,
  },
})

const currentIcon = ref('ri-file-copy-line')
const isIconClickable = ref(true)

const iconClicked = () => {
  if (isIconClickable.value) {
    isIconClickable.value = false
    currentIcon.value = 'ri-clipboard-fill'
    navigator.clipboard.writeText(props.value)

    setTimeout(() => {
      currentIcon.value = 'ri-file-copy-line'
      isIconClickable.value = true
    }, 500)
  }
}
</script>

<template>
  <div style="position: relative;">
    <div class="label">
      {{ props.label }}
    </div>
    <VTextField
      variant="filled"
      readonly
      :value="props.value"
      :append-inner-icon="currentIcon"
      @click:append-inner="iconClicked"
    />
  </div>
</template>

<style scoped>
.label {
  position: absolute;
  block-size: 20px;
  color: rgba(0, 0, 0, 60%);
  font-size: 0.75rem;
  inset-block-start:-20px;
  inset-inline-start: 10px;
}
</style>
