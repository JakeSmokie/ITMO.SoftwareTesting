<template>
	<b-card>
		<div class="text-left">
			<h4 class="mb-4">Новая группа</h4>

			<b-form-group label="📖 Название" label-cols="2">
				<b-form-input v-model="name" id="group-creation-name"/>
			</b-form-group>
			<b-form-group label="🧭 Назначение" label-cols="2">
				<b-form-input v-model="purpose" id="group-creation-purpose"/>
			</b-form-group>
		</div>
		<div class="d-flex justify-content-end">
			<b-button
				v-on:click="createGroup"
				:disabled="name.trim() === '' || purpose.trim() === ''"
				variant="outline-success"
				id="group-creation-button"
			>
				✅ Создать группу
			</b-button>
		</div>
	</b-card>
</template>
<script>
	import {mapActions} from 'vuex';

	export default {
		name: 'group-creation-form',

		data: () => ({
			name: '',
			purpose: '',
		}),

		methods: {
			...mapActions('groups', ['upsertGroup']),
			async createGroup() {
				this.upsertGroup({name: this.name, purpose: this.purpose});
				this.name = '';
				this.purpose = '';
			},
		},
	};
</script>
