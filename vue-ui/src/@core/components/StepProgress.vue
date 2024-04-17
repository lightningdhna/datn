<script setup lang="ts">
const props = defineProps({
  currentStep: {
    type: Number,
    default: 0,
  },
  items: {
    type: Array as () => Step[],
    default: () => [],
  },
  isActiveStepValid: {
    type: Boolean,
    default: false,
  },

})

const emit = defineEmits(['update:currentStep'])

interface Step {
  title: string
  subtitle: string

  // include other properties as needed
}

const cssStyle = computed(() => {
  return {
    '--active-color': '#007bff',
    '--inactive-color': '#969696',
    '--invalid-color': '#ff0000',
    '--bubble-size': '30px',
    '--line-bigger': '150%',
    '--line-delay-animation': '0.3s',
  }
})

watchEffect(() => {
  emit('update:currentStep', props.currentStep)
})
</script>

<template>
  <div
    :style="cssStyle"
    class="steps-container"
  >
    <ul class="steps-list">
      <li
        v-for="(step, index) in props.items"
        :key="index"
        class="step "
        :class="
          index === props.currentStep
            ? (props.isActiveStepValid ? 'step-active' : 'step-invalid')
            : {
              'step-passed': index < props.currentStep,
              'step-inactive': index > props.currentStep,
            }"
      >
        <div class="step-bubble ">
          <VIcon
            :key="index === props.currentStep
              ? (props.isActiveStepValid ? 'ri-circle-fill' : 'ri-error-warning-line')
              : (index < props.currentStep ? 'ri-check-line' : 'ri-circle-fill')"
            class="step-icon"
            :icon="index === props.currentStep
              ? (props.isActiveStepValid ? 'ri-circle-fill' : 'ri-error-warning-line')
              : (index < props.currentStep ? 'ri-check-line' : 'ri-circle-fill')"
          />
        </div>
        <div
          style="margin-inline :5px;"
          class="step-text"
        >
          <h5 class="text-h4 font-weight-semibold mb-1">
            {{ String(index + 1).padStart(2, '0') }}
          </h5>
        </div>
        <div
          class="step-text"
          style="max-inline-size: 45%;"
        >
          <H6 class="text-h6 font-weight-semibold mb-1">
            {{ step.title }}
          </H6>
          <H6 class="text-subtitle-1 font-weight-lighter mb-1">
            {{ step.subtitle }}
          </H6>
        </div>
        <div class="step-line">
          <div class="step-line-inner" />
        </div>
      </li>
    </ul>
  </div>
</template>

<style scoped lang="scss">
@keyframes grow {
  0% {
    transform: scale(0);
  }

  100% {
    transform: scale(1);
  }
}

.steps-container {
  inline-size: 95%;
  margin-block: 0;
  margin-inline: auto;
}

.steps-list {
  display: flex;
  list-style: none;
}

.step {
  position: relative;
  display: flex;
  flex: auto;
  flex-grow: 1;
  align-items: center;
  block-size: 60px;

  .step-line {
    z-index: 10;
    display: flex;
    align-items: center;
    background-color: var(--inactive-color);
    block-size: 5px;
    inline-size: 40%;
    inset-block-start: calc(100% - 30px);
    inset-inline-start: 0;
    margin-inline: 10px;
    transform: translateY(-50%);

    .step-line-inner{
      position: relative;
      background-color: var(--active-color);
      block-size: var(--line-bigger);
      inline-size: 0%;
      transition: all var(--line-delay-animation)  ease;
    }

  }

  &-bubble {
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 50%;
    aspect-ratio: 1/1;
    background-color: var(--inactive-color);
    block-size: var(--bubble-size);
    inline-size: var(--bubble-size);
    transition: all .3s ease;
    transition-delay: var(--line-delay-animation) -0.1s;
  }

  &-passed {
    .step-bubble {
      background-color: var(--active-color);
    }

    .step-line{
      .step-line-inner{
        inline-size: 100%;
      }
    }

  }

  &-icon {
    animation: grow .5s ease;
    background-color: white;
    block-size: calc(var(--bubble-size) - 4px);
    inline-size: calc(var(--bubble-size) - 4px);

  }

  &-invalid {
    .step-bubble {
      background-color: var(--invalid-color);

    }

    .step-text *{
      color: red
    }
  }

  &-active {
    .step-bubble {
      background-color: var(--active-color);
    }
  }

  &-inactive {
    .step-bubble {
      background-color: var(--inactive-color);
    }

    .step-line{
      .step-line-inner{
        inline-size: 0%;
      }
    }
  }

  &:last-child{
    flex-grow:0;

    .step-line{
      display:none;
    }
  }

}
</style>
