<script setup lang="ts">
import axios from 'axios'
import { useRouter } from 'vue-router'
import { useTheme } from 'vuetify'
import { VForm } from 'vuetify/components/VForm'
import AuthProvider from '@/views/pages/authentication/AuthProvider.vue'

import { MyAuthenticator } from '@/services/auth/authenticator'
import logo from '@images/logo.svg?raw'
import authV1MaskDark from '@images/pages/auth-v1-mask-dark.png'
import authV1MaskLight from '@images/pages/auth-v1-mask-light.png'
import authV1Tree2 from '@images/pages/auth-v1-tree-2.png'
import authV1Tree from '@images/pages/auth-v1-tree.png'

axios.interceptors.request.use(config => {
  const token = localStorage.getItem('token')
  if (token)
    config.headers.Authorization = `Bearer ${token}`

  return config
}, error => {
  return Promise.reject(error)
})

const form = ref({
  username: '',
  password: '',
  remember: false,
})

const requiredValidator = (value: string) => !!value || 'Required.'
const passwordErrorString = ref<string>('')
const usernameErrorString = ref<string>('')

const vuetifyTheme = useTheme()

const authThemeMask = computed(() => {
  return vuetifyTheme.global.name.value === 'light'
    ? authV1MaskLight
    : authV1MaskDark
})

const isPasswordVisible = ref(false)

const router = useRouter()

const login = async () => {
  const authenticator = new MyAuthenticator()

  console.log('form.value:', form.value)
  authenticator.authenticate(form.value.username, form.value.password)
    .then((token: string) => {
    // The authentication was successful. You can now use the token for subsequent requests.
      console.log('Authenticated successfully:', token)
      localStorage.setItem('token', token)
      localStorage.setItem('isLoggedIn', 'true')
      router.push('/')
    })
    .catch((error: Error) => {
      // An error occurred during authentication. You can handle the error here.
      if (error.message === 'Username not found') {
        usernameErrorString.value = 'Username not found'
        passwordErrorString.value = ''
      }
      else if (error.message === 'Wrong password') {
        passwordErrorString.value = 'wrong password'
        usernameErrorString.value = ''
        console.error('The password you entered is incorrect.')
      }
      else {
        usernameErrorString.value = 'Username not found'
        console.error('An error occurred during authentication:', error.message)
      }
    })

  if (form.value.remember) {
    localStorage.setItem('username', form.value.username)
    localStorage.setItem('password', form.value.password)
  }
  else {
    localStorage.removeItem('username')
    localStorage.removeItem('password')
  }
}

onMounted(() => {
  form.value.username = localStorage.getItem('username') ?? ''
  form.value.password = localStorage.getItem('password') ?? ''
  form.value.remember = !!localStorage.getItem('username') && !!localStorage.getItem('password')
})
</script>

<template>
  <!-- eslint-disable vue/no-v-html -->

  <div class="auth-wrapper d-flex align-center justify-center pa-4">
    <VCard
      class="auth-card pa-4 pt-7"
      max-width="448"
    >
      <VCardItem class="justify-center">
        <template #prepend>
          <div class="d-flex">
            <div v-html="logo" />
          </div>
        </template>

        <VCardTitle class="font-weight-semibold text-2xl text-uppercase">
          Welcome back! 👋🏻
        </VCardTitle>
      </VCardItem>

      <VCardText class="pt-2">
        <h5 class="text-h5 font-weight-semibold mb-1">
          Hệ thống quản lý in ấn! 👋🏻
        </h5>
        <p class="mb-0">
          Đăng nhập để bắt đầu quản lý công việc của bạn .
        </p>
      </VCardText>

      <VCardText>
        <VForm @submit.prevent="() => {}">
          <VRow>
            <!-- username -->
            <VCol cols="12">
              <VTextField
                v-model="form.username"
                label="Username"
                type="username"
                :rules="[requiredValidator]"
                :error-messages="usernameErrorString"
              />
            </VCol>

            <!-- password -->
            <VCol cols="12">
              <VTextField
                v-model="form.password"
                label="Password"
                placeholder="············"
                :type="isPasswordVisible ? 'text' : 'password'"
                :append-inner-icon="isPasswordVisible ? 'ri-eye-off-line' : 'ri-eye-line'"
                :rules="[requiredValidator]"
                :error-messages="passwordErrorString"
                @click:append-inner="isPasswordVisible = !isPasswordVisible"
              />

              <!-- remember me checkbox -->
              <div class="d-flex align-center justify-space-between flex-wrap mt-1 mb-4">
                <VCheckbox
                  v-model="form.remember"
                  label="Remember me"
                />

                <a
                  class="ms-2 mb-1"
                  href="javascript:void(0)"
                >
                  Forgot Password?
                </a>
              </div>

              <!-- login button -->
              <VBtn
                block
                type="submit"
                @click="login"
              >
                Login
              </VBtn>
            </VCol>

            <!-- create account -->
            <VCol
              cols="12"
              class="text-center text-base"
            >
              <span>New on our platform?</span>
              <RouterLink
                class="text-primary ms-2"
                to="/login"
              >
                Create an account
              </RouterLink>
            </VCol>

            <VCol
              cols="12"
              class="d-flex align-center"
            >
              <VDivider />
              <span class="mx-4">or</span>
              <VDivider />
            </VCol>

            <!-- auth providers -->
            <VCol
              cols="12"
              class="text-center"
            >
              <AuthProvider />
            </VCol>
          </VRow>
        </VForm>
      </VCardText>
    </VCard>

    <VImg
      class="auth-footer-start-tree d-none d-md-block"
      :src="authV1Tree"
      :width="250"
    />

    <VImg
      :src="authV1Tree2"
      class="auth-footer-end-tree d-none d-md-block"
      :width="350"
    />

    <!-- bg img -->
    <VImg
      class="auth-footer-mask d-none d-md-block"
      :src="authThemeMask"
    />
  </div>
</template>

<style lang="scss">
@use "@core/scss/pages/page-auth.scss";
</style>
