<script setup lang="ts">
import StepProgress from './StepProgress.vue'

const props = defineProps({
  onSubmit: {
    type: Function,
  },
  onValidateSuccess: {
    type: Function,
  },
  onValidateFail: {
    type: Function,
  },
  validator: {
    type: Function,
    default: () => { return true },
  },
  onFinish: {
    type: Function,
  },
  numberedSteps: {
    type: Array,
    required: true,
  },

})

const isCurrentStepValid = ref(true)
const currentStep = ref(0)
const stepLength = props.numberedSteps.length

const nextStep = async () => {
  props.onSubmit?.()
  if (!(await props.validator?.(currentStep.value))) {
    console.log('validator failed')
    isCurrentStepValid.value = false
    props.onValidateFail?.()

    return false
  }
  isCurrentStepValid.value = true
  props.onValidateSuccess?.()

  currentStep.value++

  return true
}

const onFinish = async () => {
  if (await nextStep())
    props.onFinish?.()
}

const backStep = () => {
  if (currentStep.value > 0)
    currentStep.value--
  isCurrentStepValid.value = true
}
</script>

<template>
  <VCard>
    <VCardText>
      <StepProgress
        v-model:current-step="currentStep"
        :items="props.numberedSteps"
        :is-active-step-valid="isCurrentStepValid"
      />
    </VCardText>

    <VDivider />

    <VCardText>
      <VWindow
        v-model="currentStep"
        class="disable-tab-transition"
      >
        <slot />
      </VWindow>
    </VCardText>

    <VCardText>
      <VCol cols="12">
        <div class="d-flex flex-wrap gap-4 justify-sm-space-between justify-center mt-8">
          <VBtn
            v-if="currentStep > 0"
            color="secondary"
            variant="tonal"
            class="button-prev"
            @click="backStep"
          >
            <VIcon
              icon="ri-arrow-left-line"
              start
              class="flip-in-rtl button-next"
            />
            Previous
          </VBtn>
          <VBtn
            v-else
            color="secondary"
            variant="outlined"
            disabled
          >
            <VIcon
              icon="ri-arrow-left-line"
              start
              class="flip-in-rtl"
            />
            Previous
          </VBtn>

          <VBtn
            v-if="currentStep < stepLength - 1"
            @click="nextStep"
          >
            Next
            <VIcon
              icon="ri-arrow-right-line"
              end
              class="flip-in-rtl"
            />
          </VBtn>

          <VBtn
            v-else
            color="success"
            @click="onFinish"
          >
            submit
          </VBtn>
        </div>
      </VCol>
    </VCardText>
  </VCard>
</template>
